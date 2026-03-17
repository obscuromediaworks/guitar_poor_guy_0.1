# Wwise 2024 - Buenas prácticas para juego de ritmo

## 1. Convenciones de naming

### Events
- `Play_Music_<SongId>`
- `Stop_Music_<SongId>`
- `Play_SFX_Hit`
- `Play_SFX_Miss`
- `Play_SFX_LanePress`
- `Play_SFX_Strum`
- `Play_UI_Click`

### Buses
- `Bus_Master`
- `Bus_Music`
- `Bus_SFX`
- `Bus_UI`


> Recomendación práctica: dispara `Play_SFX_LanePress` al input del jugador (aunque falle el hit) para validar táctilmente que la pulsación entró.

### RTPC
- `RTPC_Player_ComboIntensity` (0-100)
- `RTPC_Gameplay_Health` (0-100)
- `RTPC_Music_GuitarLayer` (0-100)

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


## 7. Implementación recomendada para canción por tracks (guitarra/bajo/voz/batería)

1. En Wwise, crea un Music Container con 4 tracks sincronizados: `Guitar`, `Bass`, `Vocal`, `Drums`.
2. Deja `Guitar` con volumen base más bajo que los demás (como ya hiciste).
3. Crea evento de input: `Play_SFX_LanePress` (cada pulsación).
4. Crea evento de acierto: `Play_SFX_Strum` (solo Good/Perfect).
5. Crea RTPC `RTPC_Music_GuitarLayer` y mapea el volumen del track `Guitar`:
   - 0 = guitarra más baja
   - 100 = guitarra destacada
6. En Unity:
   - `PlayLanePress(lane)` -> post event `Play_SFX_LanePress`.
   - `PlayStrum(quality)` -> post event `Play_SFX_Strum` cuando quality != Miss.
   - `SetGuitarTrackLevel(normalized)` -> set RTPC `RTPC_Music_GuitarLayer` (0..100).
7. Lógica sugerida:
   - Miss -> bajar RTPC de guitarra (ej. 10).
   - Good -> subir a ~70.
   - Perfect -> subir a ~100.

Con esto el jugador percibe que “está tocando bien” porque la guitarra del tema gana presencia al acertar.
