# Unity 6.3 + Wwise 2025.1.3: arranque paso a paso

Esta guía conecta el código base de este repo en una escena mínima jugable.

## 1) Estructura importada

Se añadieron scripts en:
- `Assets/Core/Runtime/Time`
- `Assets/Gameplay/Runtime/Data`
- `Assets/Gameplay/Runtime/System`
- `Assets/Gameplay/Runtime/Session`
- `Assets/Gameplay/Runtime/Input`
- `Assets/Audio/Runtime`

## 2) Crear assets de datos

1. En Unity: `Assets > Create > GuitarPoorGuy > Chart Source`.
2. Nómbralo `Chart_song_001`.
3. Copia el JSON de `Data/ChartFormat/sample_song.chart.json` dentro de `chartJson`.
4. En Unity: `Assets > Create > GuitarPoorGuy > Song Definition`.
5. Nómbralo `Song_song_001` y asigna `songId = song_001` + `chartSource = Chart_song_001`.

## 3) Preparar escena SongPlayScene

1. Crea escena `SongPlayScene`.
2. Crea GameObject `SongTimeSource` y añade componente `SongTimeSource`.
3. Crea GameObject `AudioService` y añade `NullAudioService` (temporal hasta conectar Wwise real).
4. Crea GameObject `LaneInputSource` y añade uno de estos componentes:
   - `KeyboardLaneInputSource` (rápido, sin Input Actions).
   - `InputSystemLaneInputSource` (recomendado para escalar con gamepad/rebinds).
5. Crea GameObject `SongSessionController` y añade componente `SongSessionController`.
6. En el inspector de `SongSessionController` asigna:
   - `Song Definition` = `Song_song_001` (recomendado)
   - `Chart Source` = `Chart_song_001` (fallback si no usas SongDefinition)
   - `Time Source` = `SongTimeSource`
   - `Audio Service Behaviour` = `AudioService` (componente `NullAudioService`)
   - `Lane Input Source Behaviour` = `LaneInputSource`

> Nota: si olvidas asignar `Lane Input Source Behaviour`, `SongSessionController` intentará auto-detectar uno en la escena y, si no encuentra, añadirá un `KeyboardLaneInputSource` como fallback.

## 4) Configurar Input Actions (opcional, recomendado)

Si usas `InputSystemLaneInputSource`:
1. Crea `Input Actions Asset` (ej. `GameplayInputActions`).
2. Action Map `Gameplay`.
3. Acciones tipo Button: `Lane0..Lane4`.
4. Bindings teclado (ejemplo): A,S,D,F,G.
5. Asigna cada acción en el array `laneActions` del componente.

## 5) Prueba rápida de gameplay

1. Play Mode.
2. Presiona carriles siguiendo timestamps aproximados del chart.
3. Verifica en consola logs de hit y combo.

## 6) Conectar Wwise real (siguiente paso)

Sustituye `NullAudioService` por un `WwiseAudioService`:
- `PlaySong(songId)` -> `AkSoundEngine.PostEvent("Play_Music_<SongId>", gameObject)`
- `StopSong(songId)` -> evento stop
- `PlayHit(quality)` -> mapear `Perfect/Good/Miss` a switch/evento
- `SetComboIntensity(value)` -> RTPC `RTPC_Player_ComboIntensity`

## 7) Recomendación inmediata

Antes de UI/VFX, valida sincronía con una sola canción de 60-90s y calibra ventanas (`perfectWindowMs`, `goodWindowMs`) + offset.


## 8) HUD básica para playtest

Sigue `Docs/UnityUIStepByStep.md` para montar la UI mínima (score/combo/multiplicador/último hit/tiempo) y validar rápidamente la sensación de juego.


## 9) Pipeline por canción

Sigue `Docs/SongAuthoringStepByStep.md` para alinear Wwise (audio) + chart (gameplay) + SongDefinition (vínculo en Unity).


## 10) Lanes + herramienta visual de charts

Sigue `Docs/LanesAndChartToolStepByStep.md` para montar carriles visuales personalizables y usar la ventana de authoring de charts en el editor.


## 11) Reinicio rápido en Play Mode

En `SongSessionController` deja activo `Enable Quick Restart` y usa `Restart Key` (default `R`) para reiniciar la sesión sin salir de Play Mode.
