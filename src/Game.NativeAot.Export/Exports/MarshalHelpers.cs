using Game.Domain.Core.Model;
using Game.NativeAot.Export.Internal;
using Game.NativeAot.Export.NativeTypes;

namespace Game.NativeAot.Export.Exports;

internal static unsafe class MarshalHelpers
{
    public static StepInput ToCore(in StepInputNative native)
    {
        return new StepInput(
            native.CastId,
            native.SeedBase,
            native.PoseIndex,
            native.SampleSlot,
            native.Dt,
            native.LureDepth,
            native.LureSpeed,
            native.Tension,
            native.WaterTemp,
            native.ActionFlags);
    }

    public static SessionCreateInput ToCore(in SessionConfigNative native)
    {
        return new SessionCreateInput(
            native.CastId,
            native.SeedBase,
            native.PoseIndex,
            native.InitialSampleSlot,
            native.InitialTension,
            native.WaterTemp);
    }

    public static SessionStepInput ToCore(in SessionStepInputNative native)
    {
        return new SessionStepInput(
            native.Dt,
            native.LureDepth,
            native.LureSpeed,
            native.ActionFlags);
    }

    public static void WriteOutput(StepOutputNative* output, StepOutput stepOutput)
    {
        output->ResultCode = (int)stepOutput.ResultCode;
        output->BiteProb = stepOutput.BiteProb;
        output->WeightFactor = stepOutput.WeightFactor;
        output->TensionDelta = stepOutput.TensionDelta;
        output->FishTypeId = stepOutput.FishTypeId;
        output->IsHit = stepOutput.IsHit ? (byte)1 : (byte)0;
        output->Reserved1 = 0;
        output->Reserved2 = 0;
    }

    public static void WriteError(ErrorInfoNative* err, int code, int detail)
    {
        if (err == null)
        {
            return;
        }

        err->Code = code;
        err->Detail = detail;
    }

    public static void ZeroOutput(StepOutputNative* output)
    {
        if (output == null)
        {
            return;
        }

        output->ResultCode = (int)ResultCode.ValidationFailed;
        output->BiteProb = 0f;
        output->WeightFactor = 0f;
        output->TensionDelta = 0f;
        output->FishTypeId = 0u;
        output->IsHit = 0;
        output->Reserved1 = 0;
        output->Reserved2 = 0;
    }

    public static int Fail(StepOutputNative* output, ErrorInfoNative* err, int code, int detail)
    {
        ZeroOutput(output);
        WriteError(err, code, detail);
        return -1;
    }

    public static int Fail(ErrorInfoNative* err, int code, int detail)
    {
        WriteError(err, code, detail);
        return -1;
    }

    public static int Success(ErrorInfoNative* err)
    {
        WriteError(err, NativeErrorCodes.Success, 0);
        return 0;
    }
}
