#pragma once

#include <stdint.h>

#ifdef _WIN32
  #define API __declspec(dllimport)
#else
  #define API
#endif

#ifdef __cplusplus
extern "C" {
#endif

typedef struct StepInputNative {
    uint64_t cast_id;
    uint64_t seed_base;
    int32_t  pose_index;
    int32_t  sample_slot;
    float    dt;
    float    lure_depth;
    float    lure_speed;
    float    tension;
    float    water_temp;
    int32_t  action_flags;
} StepInputNative;

typedef struct StepOutputNative {
    int32_t  result_code;
    float    bite_prob;
    float    weight_factor;
    float    tension_delta;
    uint32_t fish_type_id;
    uint8_t  is_hit;
    uint8_t  reserved1;
    uint16_t reserved2;
} StepOutputNative;

typedef struct ErrorInfoNative {
    int32_t code;
    int32_t detail;
} ErrorInfoNative;

typedef struct SessionConfigNative {
    uint64_t cast_id;
    uint64_t seed_base;
    int32_t  pose_index;
    int32_t  initial_sample_slot;
    float    initial_tension;
    float    water_temp;
} SessionConfigNative;

typedef struct SessionStepInputNative {
    float   dt;
    float   lure_depth;
    float   lure_speed;
    int32_t action_flags;
} SessionStepInputNative;

API int32_t calc_step(
    const StepInputNative* input,
    StepOutputNative* output,
    ErrorInfoNative* err);

API int32_t session_create(
    const SessionConfigNative* config,
    uint64_t* session,
    ErrorInfoNative* err);

API int32_t session_step(
    uint64_t session,
    const SessionStepInputNative* input,
    StepOutputNative* output,
    ErrorInfoNative* err);

API int32_t session_destroy(
    uint64_t session,
    ErrorInfoNative* err);

#ifdef __cplusplus
}
#endif
