using System;
using System.Collections.Generic;
using FourShardsScript.Symbols;

namespace FourShardsScript.Ast
{
    public class AstNode
    {
        private readonly Dictionary<Type, IAnnotation> _annotations = new Dictionary<Type, IAnnotation>();
        public void Annotate(IAnnotation annotation)
        {
            _annotations[annotation.GetType()] = annotation;
        }

        public bool HasAnnotation<T>() where T : IAnnotation
        {
            return _annotations.ContainsKey(typeof(T));
        }

        public T Annotation<T>() where T : IAnnotation
        {
            return (T) _annotations[typeof(T)];
        }

    }
}