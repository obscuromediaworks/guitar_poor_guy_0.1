# Lanes UI + herramienta visual de charts (Unity)

## Objetivo
Tener en pantalla carriles visuales, notas moviéndose, y una herramienta para editar charts sin tocar JSON manualmente.

## 1) Crear theme de lanes (customizable)

1. `Assets > Create > GuitarPoorGuy > UI > Lane Visual Theme`.
2. Configura:
   - `laneCount`, `laneWidth`, `laneSpacing`, `laneHeight`
   - colores base/acento
   - `noteLeadTimeMs`, `noteTravelHeight`, `noteHeight`

Con esto puedes re-skinnear lanes sin tocar código.

## 2) Montar UI de lanes en escena

1. En `SongPlayScene`, dentro del `Canvas`, crea `LanesRoot` (RectTransform centrado).
2. Crea objeto `LaneLayoutController` y añade componente `LaneLayoutController`.
3. Asigna:
   - `Lanes Root` = `LanesRoot`
   - `Theme` = tu `LaneVisualTheme`
4. Ejecuta contexto `Build Lanes` (botón del componente).

## 3) Mostrar notas del chart

1. Crea objeto `LaneNoteScroller` y añade componente `LaneNoteScroller`.
2. Asigna:
   - `Session Controller` = `SongSessionController`
   - `Lane Layout Controller` = `LaneLayoutController`
3. Ajusta `Note Fall Start Delay Ms` para customizar cuándo empiezan a caer las notas.
4. Inicia Play Mode: las notas del chart se renderizan por lane y bajan hacia la hit line.

## 3.1) Countdown Ready?/GO!

1. En el Canvas crea un `Text` centrado (`CountdownText`).
2. Crea un objeto `CountdownSequenceUI` y añade el componente `CountdownSequenceUI`.
3. Asigna `Countdown Text` = `CountdownText`.
4. Configura tiempos:
   - `Ready Duration Seconds`
   - `Pause Duration Seconds`
   - `Go Duration Seconds`
5. En `LaneNoteScroller`, asigna `Countdown Sequence UI` al objeto anterior.

> Si `Note Fall Start Delay Ms` está en 0, `LaneNoteScroller` toma automáticamente la duración total del countdown.

## 4) Herramienta visual para editar charts

1. Abre menú: `GuitarPoorGuy > Chart Authoring Window`.
2. Asigna un `ChartSource`.
3. Click en `Load`.
4. Edita metadata (`songId`, `bpm`, `offsetMs`) y notas.
5. Usa `Add Note`, `Delete`, `Sort by Time`.
6. Click en `Save` para persistir JSON en el `ChartSource`.

## 5) Flujo recomendado de iteración

1. Edita notas en `Chart Authoring Window`.
2. Guarda.
3. Vuelve a Play Mode y valida lanes + hit windows.
4. Ajusta offset/timing hasta que se sienta bien con el audio.

## 6) Próximos upgrades sugeridos

- Ghost notes y grilla de compases.
- Soporte completo de acordes visuales multi-lane.
- Herramienta de snap musical (1/4, 1/8, 1/16).
- Migración a TMP para HUD/lane labels con estilo final.


## 7) Troubleshooting

- Si ves lanes pero sin notas, verifica que `SongSessionController` tenga `SongDefinition`/`ChartSource` válido y que el chart tenga `notes`.
- En `LaneNoteScroller`, usa `Rebuild Notes` para forzar reconstrucción manual.
- El `LaneNoteScroller` ahora reintenta construir notas en runtime hasta que la sesión esté lista.


## 8) Zona de validación (debug)

En `LaneVisualTheme` activa `Show Validation Zone` y ajusta:
- `Validation Zone Height`
- `Validation Zone Color`

Eso dibuja una banda visible alrededor de la hit line para debug de timing/ventanas de acierto.


## 9) Reiniciar sesión en Play Mode

- En `SongSessionController` activa `Enable Quick Restart` y define `Restart Key` (default `R`).
- Durante Play Mode presiona esa tecla para reiniciar la partida sin salir del editor.
- También puedes usar el ContextMenu `Restart Session` en el componente.
