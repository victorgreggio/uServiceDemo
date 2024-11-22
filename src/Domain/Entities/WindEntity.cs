using System;
using AGTec.Common.Domain.Entities;
using uServiceDemo.Domain.Enums;

namespace uServiceDemo.Domain.Entities;

public class WindEntity : Entity
{
    public WindEntity(Guid id) : base(id)
    {
    }

    public uint Speed { set; get; }
    public WindDirection Direction { get; set; }
}