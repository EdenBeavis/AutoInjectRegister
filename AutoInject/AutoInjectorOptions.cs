﻿namespace AutoInject
{
    public class AutoInjectorOptions
    {
        public IEnumerable<Type> TypesToScan { get; set; } = [];
        public IEnumerable<Type> TypesToExclude { get; set; } = [];
    }
}