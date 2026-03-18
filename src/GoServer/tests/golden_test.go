package tests

import (
	"encoding/json"
	"os"
	"path/filepath"
	"testing"

	"nativeaot-go-demo/src/GoServer/internal/nativecalc"
)

type goldenFile struct {
	Cases []goldenCase `json:"cases"`
}

type goldenCase struct {
	Name     string         `json:"name"`
	Input    goldenInput    `json:"input"`
	Expected goldenExpected `json:"expected"`
}

type goldenInput struct {
	CastID      uint64  `json:"castId"`
	SeedBase    uint64  `json:"seedBase"`
	PoseIndex   int32   `json:"poseIndex"`
	SampleSlot  int32   `json:"sampleSlot"`
	Dt          float32 `json:"dt"`
	LureDepth   float32 `json:"lureDepth"`
	LureSpeed   float32 `json:"lureSpeed"`
	Tension     float32 `json:"tension"`
	WaterTemp   float32 `json:"waterTemp"`
	ActionFlags int32   `json:"actionFlags"`
}

type goldenExpected struct {
	ResultCode   int32   `json:"resultCode"`
	BiteProb     float32 `json:"biteProb"`
	WeightFactor float32 `json:"weightFactor"`
	TensionDelta float32 `json:"tensionDelta"`
	FishTypeID   uint32  `json:"fishTypeId"`
	IsHit        bool    `json:"isHit"`
}

func TestGoldenCases(t *testing.T) {
	cases := loadGoldenCases(t)
	for _, tc := range cases {
		tc := tc
		t.Run(tc.Name, func(t *testing.T) {
			out, err := nativecalc.CalcStep(nativecalc.StepInput{
				CastID:      tc.Input.CastID,
				SeedBase:    tc.Input.SeedBase,
				PoseIndex:   tc.Input.PoseIndex,
				SampleSlot:  tc.Input.SampleSlot,
				Dt:          tc.Input.Dt,
				LureDepth:   tc.Input.LureDepth,
				LureSpeed:   tc.Input.LureSpeed,
				Tension:     tc.Input.Tension,
				WaterTemp:   tc.Input.WaterTemp,
				ActionFlags: tc.Input.ActionFlags,
			})
			if err != nil {
				t.Fatalf("CalcStep returned error: %v", err)
			}

			assertClose(t, tc.Expected.BiteProb, out.BiteProb)
			assertClose(t, tc.Expected.WeightFactor, out.WeightFactor)
			assertClose(t, tc.Expected.TensionDelta, out.TensionDelta)

			if out.ResultCode != tc.Expected.ResultCode {
				t.Fatalf("resultCode mismatch: want %d got %d", tc.Expected.ResultCode, out.ResultCode)
			}

			if out.FishTypeID != tc.Expected.FishTypeID {
				t.Fatalf("fishTypeId mismatch: want %d got %d", tc.Expected.FishTypeID, out.FishTypeID)
			}

			if out.IsHit != tc.Expected.IsHit {
				t.Fatalf("isHit mismatch: want %v got %v", tc.Expected.IsHit, out.IsHit)
			}
		})
	}
}

func TestSessionStep_AdvancesSampleSlotAndTension(t *testing.T) {
	session, err := nativecalc.CreateSession(nativecalc.SessionConfig{
		CastID:            10001,
		SeedBase:          20260318,
		PoseIndex:         3,
		InitialSampleSlot: 12,
		InitialTension:    18,
		WaterTemp:         19,
	})
	if err != nil {
		t.Fatalf("CreateSession returned error: %v", err)
	}
	defer func() {
		if closeErr := session.Close(); closeErr != nil {
			t.Fatalf("Close returned error: %v", closeErr)
		}
	}()

	first, err := session.Step(nativecalc.SessionStepInput{
		Dt:          0.016,
		LureDepth:   5.2,
		LureSpeed:   2.7,
		ActionFlags: 1,
	})
	if err != nil {
		t.Fatalf("first Step returned error: %v", err)
	}

	second, err := session.Step(nativecalc.SessionStepInput{
		Dt:          0.016,
		LureDepth:   5.2,
		LureSpeed:   2.7,
		ActionFlags: 1,
	})
	if err != nil {
		t.Fatalf("second Step returned error: %v", err)
	}

	assertClose(t, 0.5717213, first.BiteProb)
	assertClose(t, 10.740984, first.TensionDelta)

	if first.FishTypeID != 2001 {
		t.Fatalf("first fishTypeId mismatch: want 2001 got %d", first.FishTypeID)
	}

	if second.ResultCode != 0 {
		t.Fatalf("second resultCode mismatch: want 0 got %d", second.ResultCode)
	}

	if first == second {
		t.Fatalf("expected stateful second step to differ from first output")
	}
}

func loadGoldenCases(t *testing.T) []goldenCase {
	t.Helper()

	root := filepath.Clean(filepath.Join("..", "..", "..", "tests", "GoldenCases", "step_cases.json"))
	bytes, err := os.ReadFile(root)
	if err != nil {
		t.Fatalf("read golden cases: %v", err)
	}

	var file goldenFile
	if err := json.Unmarshal(bytes, &file); err != nil {
		t.Fatalf("parse golden cases: %v", err)
	}

	return file.Cases
}

func assertClose(t *testing.T, want, got float32) {
	t.Helper()

	diff := want - got
	if diff < 0 {
		diff = -diff
	}

	if diff > 0.0001 {
		t.Fatalf("float mismatch: want %.6f got %.6f", want, got)
	}
}
