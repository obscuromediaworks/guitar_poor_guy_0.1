# Song authoring pipeline (Wwise + Chart) - paso a paso

Esta guía define dónde se configura cada cosa por canción:
- **Wwise**: qué audio se reproduce.
- **Chart**: qué toca el jugador.
- **Unity SongDefinition**: vínculo entre ambos.

## 1) Preparar audio en Wwise

1. Importa tus tracks/stems de la canción.
2. Arma tu playlist/music hierarchy.
3. Crea eventos:
   - `Play_Music_<songId>`
   - `Stop_Music_<songId>`
4. Genera SoundBanks para la plataforma objetivo.

## 2) Preparar chart de gameplay

1. Crea o edita JSON de chart con formato del proyecto.
2. Define:
   - `songId`
   - `bpm`
   - `offsetMs`
   - `notes[]` con `timeMs`, `lane`, `type`.
3. Copia ese JSON en un `ChartSource` de Unity.

## 3) Crear SongDefinition en Unity

1. `Assets > Create > GuitarPoorGuy > Song Definition`.
2. Asigna:
   - `songId` (igual al de Wwise)
   - `chartSource` (tu ChartSource)
3. En `SongSessionController`, asigna `songDefinition`.

## 4) Regla práctica

- Si `SongDefinition` está asignado, el juego usa ese `songId` y ese chart.
- Si no está asignado, usa fallback al `ChartSource` directo del controlador.

## 5) Checklist rápido por canción

- [ ] `songId` coincide entre Wwise y Unity.
- [ ] Evento `Play_Music_<songId>` existe en Wwise.
- [ ] Chart tiene notas válidas y offset calibrado.
- [ ] Se reproduce audio correcto al iniciar sesión.
- [ ] Input/hits corresponden a las notas del chart.


## 6) Paso a paso Unity + Wwise para tu canción de 4 tracks

1. Verifica que `songId` sea idéntico entre:
   - `SongDefinition.songId`
   - `chart.songId`
   - evento Wwise `Play_Music_<songId>`
2. En `SongSessionController` asigna tu `SongDefinition`.
3. En tu `WwiseAudioService` implementa:
   - `PlayLanePress(lane)` -> `Play_SFX_LanePress`
   - `PlayStrum(quality)` -> `Play_SFX_Strum` para Good/Perfect
   - `SetGuitarTrackLevel(normalized)` -> `RTPC_Music_GuitarLayer`
4. En juego, valida:
   - al presionar tecla siempre suena lane press
   - al acertar suena strum
   - el track de guitarra sube al acertar y baja al fallar
5. Ajusta la sensibilidad musical:
   - Good = 0.7
   - Perfect = 1.0
   - Miss = 0.1

Esto te da el efecto de “guitarra del jugador” sobre el backing track.
