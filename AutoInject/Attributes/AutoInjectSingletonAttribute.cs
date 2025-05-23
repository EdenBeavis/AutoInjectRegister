﻿using AutoInject.Enums;
using Microsoft.Extensions.DependencyInjection;

namespace AutoInject.Attributes;

public class AutoInjectSingletonAttribute() : AutoInjectAttribute(ServiceLifetime.Singleton, AddType.Add)
{
}