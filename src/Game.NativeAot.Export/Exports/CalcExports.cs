using System.Runtime.InteropServices;
using Game.Domain.Core.Simulation;
using Game.NativeAot.Export.Internal;
using Game.NativeAot.Export.NativeTypes;

namespace Game.NativeAot.Export.Exports;

public static unsafe class CalcExports
{
    [UnmanagedCallersOnly(EntryPoint = "calc_step")]
    public static int CalcStep(StepInputNative* input, StepOutputNative* output, ErrorInfoNative* err)
    {
        if (input == null || output == null || err == null)
        {
            return MarshalHelpers.Fail(output, err, NativeErrorCodes.NullPointer, 0);
        }

        try
        {
            var coreInput = MarshalHelpers.ToCore(*input);
            var result = RealtimeStepCalculator.Step(in coreInput);
            if (!result.IsSuccess)
            {
                return MarshalHelpers.Fail(output, err, NativeErrorCodes.ValidationFailed, (int)result.ValidationError);
            }

            MarshalHelpers.WriteOutput(output, result.Output);
            return MarshalHelpers.Success(err);
        }
        catch
        {
            return MarshalHelpers.Fail(output, err, NativeErrorCodes.UnexpectedFailure, 0);
        }
    }
}
