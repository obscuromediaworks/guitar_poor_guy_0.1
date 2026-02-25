# Arquitectura recomendada (Unity 6 + Wwise)

## 1. Capas del proyecto

### Core
Responsabilidades transversales:
- Gestión de tiempo global (`ITimeSource`).
- Event bus interno (`IGameEventBus`).
- Configuración de juego (`GameConfig`).
- Carga de escenas y estado de sesión.

### Gameplay
- `SongSessionController`: orquesta reproducción de canción y estado de partida.
- `ChartPlayer`: interpreta eventos de chart por timestamp.
- `LaneInputSystem`: procesa input por carril.
- `HitJudge`: evalúa ventanas de acierto (Perfect/Good/Miss).
- `ScoreSystem`: score, combo, multiplicador y streak.
- `NotePool`: pooling de objetos de nota.

### Audio (Adaptador Wwise)
- `AudioEventService`: wrapper para `PostEvent`.
- `MusicSyncService`: sincronización entre tiempo musical y gameplay.
- `AudioStateOrchestrator`: States/Switches globales (menu/gameplay/pause).
- `RtpcBridge`: actualización controlada de RTPC (sin spam por frame).

### UI
- HUD score/combo.
- Feedback de hit quality.
- Menús de canción, pausa y resultados.

## 2. Flujo principal de una canción

1. Cargar metadata de canción y chart.
2. Preparar escena + pools.
3. Iniciar música en Wwise (`Play_Music_MainSong`).
4. `ChartPlayer` emite notas programadas según song time.
5. Input jugador -> `HitJudge` -> `ScoreSystem`.
6. Gameplay notifica audio:
   - Hit correcto: `Play_SFX_Hit`.
   - Miss: `Play_SFX_Miss`.
   - Combo: RTPC `Player_ComboIntensity`.
7. Fin de canción -> pantalla de resultados.

## 3. Reglas de escalabilidad

- Cada feature se implementa detrás de interfaces.
- Cualquier dependencia de Wwise se encapsula en módulo `Audio`.
- Nuevas mecánicas (hammer-on, star power, etc.) se agregan como sistemas independientes.
- El chart versionado permite backward compatibility:
  - `formatVersion: 1` en MVP.
  - migradores para versiones futuras.

## 4. Performance checklist (desde el inicio)

- Pooling para notas, VFX y feedback UI temporal.
- Evitar LINQ en `Update`/`FixedUpdate`.
- No crear strings por frame en HUD.
- Actualizar RTPC sólo cuando cambie el valor o en intervalos discretos (p. ej. 10 Hz).
- Precalcular lookup de eventos de chart por segmento temporal.
- Limitar voces simultáneas y priorizar buses críticos en Wwise.

## 5. Sugerencia de estructura en Assets

```text
Assets/
  Core/
    Runtime/
    Config/
  Gameplay/
    Runtime/
    Data/
    Prefabs/
  Audio/
    Runtime/
    Wwise/
  UI/
    Runtime/
    Prefabs/
  Songs/
    Song_001/
      Audio/
      Charts/
      Metadata/
```
