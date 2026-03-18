using System.Runtime.InteropServices;
using Game.Domain.Core.Simulation;
using Game.NativeAot.Export.Internal;
using Game.NativeAot.Export.NativeTypes;

namespace Game.NativeAot.Export.Exports;

public static unsafe class SessionExports
{
    [UnmanagedCallersOnly(EntryPoint = "session_create")]
    public static int SessionCreate(SessionConfigNative* config, ulong* session, ErrorInfoNative* err)
    {
        if (config == null || session == null || err == null)
        {
            return MarshalHelpers.Fail(err, NativeErrorCodes.NullPointer, 0);
        }

        *session = 0;

        try
        {
            var coreConfig = MarshalHelpers.ToCore(*config);
            var validation = Game.Domain.Core.Validation.SessionValidator.Validate(in coreConfig);
            if (validation != Game.Domain.Core.Model.ValidationErrorDetail.None)
            {
                return MarshalHelpers.Fail(err, NativeErrorCodes.ValidationFailed, (int)validation);
            }

            *session = SessionRegistry.Add(new FishingSession(in coreConfig));
            return MarshalHelpers.Success(err);
        }
        catch
        {
            *session = 0;
            return MarshalHelpers.Fail(err, NativeErrorCodes.UnexpectedFailure, 0);
        }
    }

    [UnmanagedCallersOnly(EntryPoint = "session_step")]
    public static int SessionStep(ulong session, SessionStepInputNative* input, StepOutputNative* output, ErrorInfoNative* err)
    {
        if (input == null || output == null || err == null)
        {
            return MarshalHelpers.Fail(output, err, NativeErrorCodes.NullPointer, 0);
        }

        if (!SessionRegistry.TryGet(session, out var state) || state == null)
        {
            return MarshalHelpers.Fail(output, err, NativeErrorCodes.InvalidSession, 0);
        }

        try
        {
            var coreInput = MarshalHelpers.ToCore(*input);
            var result = state.Step(in coreInput);
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

    [UnmanagedCallersOnly(EntryPoint = "session_destroy")]
    public static int SessionDestroy(ulong session, ErrorInfoNative* err)
    {
        if (err == null)
        {
            return MarshalHelpers.Fail(err, NativeErrorCodes.NullPointer, 0);
        }

        if (session == 0 || !SessionRegistry.Remove(session))
        {
            return MarshalHelpers.Fail(err, NativeErrorCodes.InvalidSession, 0);
        }

        return MarshalHelpers.Success(err);
    }
}
