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
