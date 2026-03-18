/////////////////////////////////////////////////////////////////////////////////////////////////////
//
// Audiokinetic Wwise generated include file. Do not edit.
//
/////////////////////////////////////////////////////////////////////////////////////////////////////

#ifndef __WWISE_IDS_H__
#define __WWISE_IDS_H__

#include <AK/SoundEngine/Common/AkTypes.h>

namespace AK
{
    namespace EVENTS
    {
        static const AkUniqueID PLAY_MUSIC_SONG_001 = 1966749849U;
        static const AkUniqueID PLAY_SFX_HIT_001 = 681554853U;
        static const AkUniqueID PLAY_SFX_LANEPRESS = 1856456021U;
        static const AkUniqueID PLAY_SFX_MISS_000 = 17380391U;
        static const AkUniqueID PLAY_SFX_STRUM = 4014817549U;
        static const AkUniqueID PLAY_SFX_UI_CLICK = 3504421513U;
        static const AkUniqueID STOP_MUSIC_SONG_001 = 3884991287U;
    } // namespace EVENTS

    namespace STATES
    {
        namespace GAMEFLOW
        {
            static const AkUniqueID GROUP = 392736999U;

            namespace STATE
            {
                static const AkUniqueID GAMEPLAY = 89505537U;
                static const AkUniqueID MENU = 2607556080U;
                static const AkUniqueID NONE = 748895195U;
                static const AkUniqueID PAUSE = 3092587493U;
                static const AkUniqueID RESULTS = 3780578133U;
            } // namespace STATE
        } // namespace GAMEFLOW

    } // namespace STATES

    namespace SWITCHES
    {
        namespace HITQUALITY
        {
            static const AkUniqueID GROUP = 2990443U;

            namespace SWITCH
            {
                static const AkUniqueID GOOD = 668632890U;
                static const AkUniqueID MISS = 3062523241U;
                static const AkUniqueID PERFECT = 2161557176U;
            } // namespace SWITCH
        } // namespace HITQUALITY

    } // namespace SWITCHES

    namespace GAME_PARAMETERS
    {
        static const AkUniqueID RTPC_MUSIC_GUITARLAYER = 2455186356U;
    } // namespace GAME_PARAMETERS

    namespace BANKS
    {
        static const AkUniqueID INIT = 1355168291U;
        static const AkUniqueID GPG_MAIN = 531985781U;
    } // namespace BANKS

    namespace BUSSES
    {
        static const AkUniqueID BUS_MASTER = 3964155266U;
        static const AkUniqueID BUS_MUSIC = 1162281553U;
        static const AkUniqueID BUS_SFX = 3895923845U;
        static const AkUniqueID BUS_UI = 1746463750U;
    } // namespace BUSSES

    namespace AUDIO_DEVICES
    {
        static const AkUniqueID NO_OUTPUT = 2317455096U;
        static const AkUniqueID SYSTEM = 3859886410U;
    } // namespace AUDIO_DEVICES

}// namespace AK

#endif // __WWISE_IDS_H__
