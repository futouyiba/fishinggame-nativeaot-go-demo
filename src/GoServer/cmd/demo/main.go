package main

import (
	"fmt"
	"log"

	"nativeaot-go-demo/src/GoServer/internal/nativecalc"
)

func main() {
	output, err := nativecalc.CalcStep(nativecalc.StepInput{
		CastID:      10001,
		SeedBase:    20260318,
		PoseIndex:   3,
		SampleSlot:  12,
		Dt:          0.016,
		LureDepth:   5.2,
		LureSpeed:   2.7,
		Tension:     18,
		WaterTemp:   19,
		ActionFlags: 1,
	})
	if err != nil {
		log.Fatal(err)
	}

	fmt.Printf("result=%+v\n", output)

	session, err := nativecalc.CreateSession(nativecalc.SessionConfig{
		CastID:            10001,
		SeedBase:          20260318,
		PoseIndex:         3,
		InitialSampleSlot: 12,
		InitialTension:    18,
		WaterTemp:         19,
	})
	if err != nil {
		log.Fatal(err)
	}
	defer func() {
		if closeErr := session.Close(); closeErr != nil {
			log.Fatal(closeErr)
		}
	}()

	sessionOutput, err := session.Step(nativecalc.SessionStepInput{
		Dt:          0.016,
		LureDepth:   5.2,
		LureSpeed:   2.7,
		ActionFlags: 1,
	})
	if err != nil {
		log.Fatal(err)
	}

	fmt.Printf("session_result=%+v\n", sessionOutput)
}
