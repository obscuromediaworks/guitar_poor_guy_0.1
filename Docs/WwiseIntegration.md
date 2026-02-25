# Wwise 2024 - Buenas prácticas para juego de ritmo

## 1. Convenciones de naming

### Events
- `Play_Music_<SongId>`
- `Stop_Music_<SongId>`
- `Play_SFX_Hit`
- `Play_SFX_Miss`
- `Play_UI_Click`

### Buses
- `Bus_Master`
- `Bus_Music`
- `Bus_SFX`
- `Bus_UI`

### RTPC
- `RTPC_Player_ComboIntensity` (0-100)
- `RTPC_Gameplay_Health` (0-100)

### States / Switches
- State Group `GameFlow`: `Menu`, `Gameplay`, `Pause`, `Results`
- Switch Group `HitQuality`: `Perfect`, `Good`, `Miss`

## 2. Diseño de audio para MVP

- Música principal en `Bus_Music`.
- SFX de hit/miss cortos y con límite de voces.
- UI independiente en `Bus_UI` para control de volumen separado.

## 3. Sincronía musical

- Usa un único origen temporal (song time) para spawn y juicio.
- No mezclar varios relojes sin calibración.
- Si hay latencia de dispositivo, exponer un `InputOffsetMs` configurable por jugador.

## 4. Integración técnica recomendada

- Encapsular llamadas Wwise en un servicio (`IAudioService`).
- Evitar `PostEvent` directo desde scripts de gameplay dispersos.
- Crear mapa de eventos en configuración (ScriptableObject o tabla estática).

## 5. Optimización Wwise

- Activar virtual voices en SFX no críticos.
- Limitar polyphony para eventos de spam (hit rápido).
- Revisar Profiler de Wwise durante canciones densas.
- Cargar bancos por escena/canción para minimizar memoria pico.

## 6. Checklist de release interno

- [ ] Todos los eventos usados en Unity existen en Wwise.
- [ ] No hay warnings de banks faltantes al iniciar canción.
- [ ] Volumen relativo Music/SFX/UI calibrado.
- [ ] CPU audio estable durante secciones densas.
