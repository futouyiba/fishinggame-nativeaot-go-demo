package nativecalc

/*
#cgo windows CFLAGS: -I${SRCDIR}
#cgo windows LDFLAGS: -L${SRCDIR}/lib -lGameNative
#include "bridge.h"
*/
import "C"

import "fmt"

type StepInput struct {
	CastID      uint64
	SeedBase    uint64
	PoseIndex   int32
	SampleSlot  int32
	Dt          float32
	LureDepth   float32
	LureSpeed   float32
	Tension     float32
	WaterTemp   float32
	ActionFlags int32
}

type StepOutput struct {
	ResultCode   int32
	BiteProb     float32
	WeightFactor float32
	TensionDelta float32
	FishTypeID   uint32
	IsHit        bool
}

type NativeError struct {
	Op     string
	Code   int32
	Detail int32
}

func (e NativeError) Error() string {
	if e.Op == "" {
		return fmt.Sprintf("native call failed: code=%d detail=%d", e.Code, e.Detail)
	}

	return fmt.Sprintf("%s failed: code=%d detail=%d", e.Op, e.Code, e.Detail)
}

func CalcStep(in StepInput) (StepOutput, error) {
	cin := C.StepInputNative{
		cast_id:      C.uint64_t(in.CastID),
		seed_base:    C.uint64_t(in.SeedBase),
		pose_index:   C.int32_t(in.PoseIndex),
		sample_slot:  C.int32_t(in.SampleSlot),
		dt:           C.float(in.Dt),
		lure_depth:   C.float(in.LureDepth),
		lure_speed:   C.float(in.LureSpeed),
		tension:      C.float(in.Tension),
		water_temp:   C.float(in.WaterTemp),
		action_flags: C.int32_t(in.ActionFlags),
	}

	var cout C.StepOutputNative
	var cerr C.ErrorInfoNative

	rc := C.calc_step(&cin, &cout, &cerr)
	if rc != 0 {
		return StepOutput{}, NativeError{
			Op:     "calc_step",
			Code:   int32(cerr.code),
			Detail: int32(cerr.detail),
		}
	}

	return StepOutput{
		ResultCode:   int32(cout.result_code),
		BiteProb:     float32(cout.bite_prob),
		WeightFactor: float32(cout.weight_factor),
		TensionDelta: float32(cout.tension_delta),
		FishTypeID:   uint32(cout.fish_type_id),
		IsHit:        cout.is_hit != 0,
	}, nil
}

type SessionConfig struct {
	CastID            uint64
	SeedBase          uint64
	PoseIndex         int32
	InitialSampleSlot int32
	InitialTension    float32
	WaterTemp         float32
}

type SessionStepInput struct {
	Dt          float32
	LureDepth   float32
	LureSpeed   float32
	ActionFlags int32
}

type Session struct {
	handle C.uint64_t
}

func CreateSession(config SessionConfig) (*Session, error) {
	cconfig := C.SessionConfigNative{
		cast_id:             C.uint64_t(config.CastID),
		seed_base:           C.uint64_t(config.SeedBase),
		pose_index:          C.int32_t(config.PoseIndex),
		initial_sample_slot: C.int32_t(config.InitialSampleSlot),
		initial_tension:     C.float(config.InitialTension),
		water_temp:          C.float(config.WaterTemp),
	}

	var handle C.uint64_t
	var cerr C.ErrorInfoNative

	rc := C.session_create(&cconfig, &handle, &cerr)
	if rc != 0 {
		return nil, NativeError{
			Op:     "session_create",
			Code:   int32(cerr.code),
			Detail: int32(cerr.detail),
		}
	}

	return &Session{handle: handle}, nil
}

func (s *Session) Step(in SessionStepInput) (StepOutput, error) {
	if s == nil || s.handle == 0 {
		return StepOutput{}, NativeError{
			Op:   "session_step",
			Code: 3000,
		}
	}

	cin := C.SessionStepInputNative{
		dt:           C.float(in.Dt),
		lure_depth:   C.float(in.LureDepth),
		lure_speed:   C.float(in.LureSpeed),
		action_flags: C.int32_t(in.ActionFlags),
	}

	var cout C.StepOutputNative
	var cerr C.ErrorInfoNative

	rc := C.session_step(s.handle, &cin, &cout, &cerr)
	if rc != 0 {
		return StepOutput{}, NativeError{
			Op:     "session_step",
			Code:   int32(cerr.code),
			Detail: int32(cerr.detail),
		}
	}

	return StepOutput{
		ResultCode:   int32(cout.result_code),
		BiteProb:     float32(cout.bite_prob),
		WeightFactor: float32(cout.weight_factor),
		TensionDelta: float32(cout.tension_delta),
		FishTypeID:   uint32(cout.fish_type_id),
		IsHit:        cout.is_hit != 0,
	}, nil
}

func (s *Session) Close() error {
	if s == nil || s.handle == 0 {
		return nil
	}

	var cerr C.ErrorInfoNative
	rc := C.session_destroy(s.handle, &cerr)
	if rc != 0 {
		return NativeError{
			Op:     "session_destroy",
			Code:   int32(cerr.code),
			Detail: int32(cerr.detail),
		}
	}

	s.handle = 0
	return nil
}
