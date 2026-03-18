namespace Game.Domain.Core.Model;

public enum ValidationErrorDetail
{
    None = 0,
    DtOutOfRange = 1,
    LureDepthOutOfRange = 2,
    LureSpeedOutOfRange = 3,
    TensionOutOfRange = 4,
    WaterTempOutOfRange = 5,
    NonFiniteValue = 6
}
