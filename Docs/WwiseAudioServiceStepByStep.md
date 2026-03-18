# WwiseAudioService en Unity - guía detallada (sin saltarse pasos)

Esta guía asume que ya tienes eventos y RTPCs creados en Wwise.

## 1) Preparación en Wwise

Verifica que existen exactamente estos nombres:

### Events
- `Play_Music_<songId>` (por canción)
- `Stop_Music_<songId>`
- `Play_SFX_LanePress`
- `Play_SFX_Hit`
- `Play_SFX_Miss`
- `Play_SFX_Strum`

### Switch / RTPC
- Switch Group: `HitQuality` con `Perfect`, `Good`, `Miss`
- RTPC: `RTPC_Player_ComboIntensity`
- RTPC: `RTPC_Music_GuitarLayer`

## 2) Configuración en Unity (escena)

1. Crea GameObject `AudioService`.
2. Añade componente `WwiseAudioService`.
3. En `SongSessionController` asigna `Audio Service Behaviour` a `AudioService`.
4. Revisa que `SongDefinition.songId` coincide con el evento `Play_Music_<songId>`.

## 3) Mapeo de comportamiento (ya implementado)

- Pulsación de tecla: `PlayLanePress(lane)` -> `Play_SFX_LanePress`.
- Hit Good/Perfect: `PlayStrum(quality)` -> `Play_SFX_Strum`.
- Cualquier hit/miss: `PlayHit(quality)` -> switch `HitQuality` + `Play_SFX_Hit/Miss`.
- Combo: `SetComboIntensity(0..1)` -> `RTPC_Player_ComboIntensity` (0..100).
- Performance guitarra:
  - Miss -> `SetGuitarTrackLevel(0.1)`
  - Good -> `SetGuitarTrackLevel(0.7)`
  - Perfect -> `SetGuitarTrackLevel(1.0)`

## 4) Troubleshooting (checklist exacto)

### A. Se ve gameplay pero no suena nada
- [ ] ¿SoundBanks generados para tu plataforma?
- [ ] ¿GameObject `AudioService` tiene `WwiseAudioService` y está asignado?
- [ ] ¿Nombres idénticos (mayúsculas/minúsculas)?

### B. Suena lane press pero no strum
- [ ] ¿`Play_SFX_Strum` existe en Wwise?
- [ ] ¿Estás acertando notas (Good/Perfect) y no todo Miss?

### C. No sube la guitarra al acertar
- [ ] ¿`RTPC_Music_GuitarLayer` está realmente mapeado al volumen del track Guitar?
- [ ] ¿Rango de RTPC en Wwise está en 0..100?
- [ ] ¿El track Guitar tiene headroom para percibirse el cambio?

### D. Input se detecta pero score no sube
- [ ] Alinea `gameplayTimingDelayMs` en `SongSessionController` con el countdown.
- [ ] Si usas countdown, deja `gameplayTimingDelayMs = 0` para auto-config (usa duración total del countdown).

## 5) Validación mínima en Play Mode

1. Presiona cualquier lane: debe sonar `LanePress`.
2. Acierta una nota: debe sonar `Strum` + subir guitarra.
3. Falla una nota: debe bajar guitarra.
4. Presiona `R` para restart: sesión reinicia sin salir de Play Mode.
