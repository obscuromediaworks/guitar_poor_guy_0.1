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
3. Inicia Play Mode: las notas del chart se renderizan por lane y bajan hacia la hit line.

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
