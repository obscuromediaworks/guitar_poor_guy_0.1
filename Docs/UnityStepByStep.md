# Unity 6.3 + Wwise 2025.1.3: arranque paso a paso

Esta guía conecta el código base de este repo en una escena mínima jugable.

## 1) Estructura importada

Se añadieron scripts en:
- `Assets/Core/Runtime/Time`
- `Assets/Gameplay/Runtime/Data`
- `Assets/Gameplay/Runtime/System`
- `Assets/Gameplay/Runtime/Session`
- `Assets/Audio/Runtime`

## 2) Crear assets de datos

1. En Unity: `Assets > Create > GuitarPoorGuy > Chart Source`.
2. Nómbralo `Chart_song_001`.
3. Copia el JSON de `Data/ChartFormat/sample_song.chart.json` dentro de `chartJson`.

## 3) Preparar escena SongPlayScene

1. Crea escena `SongPlayScene`.
2. Crea GameObject `SongTimeSource` y añade componente `SongTimeSource`.
3. Crea GameObject `AudioService` y añade `NullAudioService` (temporal hasta conectar Wwise real).
4. Crea GameObject `SongSessionController` y añade componente `SongSessionController`.
5. En el inspector de `SongSessionController` asigna:
   - `Chart Source` = `Chart_song_001`
   - `Time Source` = `SongTimeSource`
   - `Audio Service Behaviour` = `AudioService` (componente `NullAudioService`)

## 4) Prueba rápida de gameplay

1. Play Mode.
2. Presiona teclas `A,S,D,F,G` siguiendo timestamps aproximados del chart.
3. Verifica en consola logs de hit y combo.

## 5) Conectar Wwise real (siguiente paso)

Sustituye `NullAudioService` por un `WwiseAudioService`:
- `PlaySong(songId)` -> `AkSoundEngine.PostEvent("Play_Music_<SongId>", gameObject)`
- `StopSong(songId)` -> evento stop
- `PlayHit(quality)` -> mapear `Perfect/Good/Miss` a switch/evento
- `SetComboIntensity(value)` -> RTPC `RTPC_Player_ComboIntensity`

## 6) Recomendación inmediata

Antes de UI/VFX, valida sincronía con una sola canción de 60-90s y calibra ventanas (`perfectWindowMs`, `goodWindowMs`) + offset.
