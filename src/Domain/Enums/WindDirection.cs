using System.ComponentModel;

namespace uServiceDemo.Domain.Enums;

public enum WindDirection
{
    [Description("North")] N,
    [Description("Northeast")] NE,
    [Description("East")] E,
    [Description("Southeast")] SE,
    [Description("South")] S,
    [Description("Southwest")] SW,
    [Description("West")] W,
    [Description("Northwest")] NW
}