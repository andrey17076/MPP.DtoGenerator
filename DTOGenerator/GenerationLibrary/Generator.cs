using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using GenerationLibrary.JsonDescriptions;
using GenerationLibrary.TypeAssistance;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Formatting;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;
using ThreadPool = GenerationLibrary.Threading.ThreadPool;

namespace GenerationLibrary
{
    public class Generator
    {
        private readonly string _namespaceName;
        private readonly ThreadPool _threadPool;

        public Generator(int taskCount, string namespaceName)
        {
            _namespaceName = namespaceName;
            _threadPool = new ThreadPool(taskCount);
        }

        public List<DtoClass> GetDtoClassList(JsonClassDescriptionList classDescriptionList)
        {
            var dtoClassList = new ConcurrentBag<DtoClass>();
            foreach (var classDescription in classDescriptionList.ClassDescriptions)
            {
                _threadPool.QueueUserWorkItem(delegate
                {
                    try
                    {
                        dtoClassList.Add(GetDtoClass(classDescription));
                    }
                    catch (Exception exception)
                    {
                        Console.WriteLine(exception);
                    }
                });
            }
            while (dtoClassList.Count != classDescriptionList.ClassDescriptions.Length);
            return dtoClassList.ToList();
        }


        private DtoClass GetDtoClass(JsonClassDescription classDescription)
        {
            var classDeclaration = GetClassDeclaration(classDescription);
            var namespaceDeclaration = NamespaceDeclaration(IdentifierName(_namespaceName));
            namespaceDeclaration = namespaceDeclaration.WithMembers(List(new [] {classDeclaration}));
            var code = Formatter.Format(namespaceDeclaration, new AdhocWorkspace()).ToFullString();
            return new DtoClass(classDescription.ClassName, code);
        }

        private static MemberDeclarationSyntax GetClassDeclaration(JsonClassDescription classDescription)
        {
            var classDeclaration = ClassDeclaration(classDescription.ClassName);
            var publicSealedModifiers = TokenList(Token(SyntaxKind.PublicKeyword), Token(SyntaxKind.SealedKeyword));
            return classDeclaration
                .WithModifiers(publicSealedModifiers)
                .WithMembers(List(classDescription.Properties.Select(GetPropertyDeclaration)));
        }

        private static MemberDeclarationSyntax GetPropertyDeclaration(JsonPropertyDescription propertyDescription)
        {
            var typeName = GetNetType(propertyDescription.Type, propertyDescription.Format).FullName;
            var propertyDeclaration = PropertyDeclaration(IdentifierName(typeName), propertyDescription.Name);
            return propertyDeclaration
                .WithModifiers(TokenList(Token(SyntaxKind.PublicKeyword)))
                .WithAccessorList(GetSetAccessors);
        }

        private static Type GetNetType(string typeName, string formatName)
        {
            Type type;
            TypeTable.Instance.TryGetNetType(typeName, formatName, out type);
            return type;
        }

        private static AccessorListSyntax GetSetAccessors => AccessorList(
            List(
                new[]
                {
                    AccessorDeclaration(SyntaxKind.GetAccessorDeclaration)
                        .WithSemicolonToken(Token(SyntaxKind.SemicolonToken)),
                    AccessorDeclaration(SyntaxKind.SetAccessorDeclaration)
                        .WithSemicolonToken(Token(SyntaxKind.SemicolonToken))
                }
            )
        );
    }
}

