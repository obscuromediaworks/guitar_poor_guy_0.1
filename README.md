# Guitar Poor Guy 0.1

Base de planeación y arranque para un juego de ritmo estilo *Guitar Hero* (alcance inicial sencillo), construido con **Unity 6** y **Wwise 2024** con foco en:

- Integración de audio robusta.
- Arquitectura escalable por features.
- Buen rendimiento desde la versión 0.1.

## Objetivo del MVP (v0.1)

Construir una primera versión jugable con:

1. Una sola canción.
2. Un solo instrumento visual (guitarra simplificada de 5 carriles).
3. 3 tipos de nota: normal, sustain y acorde (2 notas simultáneas).
4. Sistema de score básico y multiplicador.
5. Feedback sonoro con Wwise (música, acierto/fallo, UI).

## Stack recomendado

- **Engine:** Unity 6 LTS.
- **Audio middleware:** Wwise 2024.x + paquete oficial de integración para Unity.
- **Lenguaje:** C#.
- **Control de versiones:** Git + LFS para audio pesado.
- **Target inicial:** PC (Windows) para iteración rápida.

## Estructura sugerida

```text
Docs/
  Architecture.md
  WwiseIntegration.md
  Roadmap.md
Data/
  ChartFormat/
    chart.schema.json
    sample_song.chart.json
```

> Cuando crees el proyecto Unity, replica esta organización dentro de `Assets/` con carpetas por feature (`Gameplay`, `Audio`, `UI`, `Core`).

## Primeros pasos (orden recomendado)

1. Crea proyecto nuevo en Unity 6 (3D Core o URP según objetivo visual).
2. Instala integración oficial de Wwise 2024 para Unity.
3. Define buses, eventos y states base usando `Docs/WwiseIntegration.md`.
4. Implementa loop de gameplay mínimo:
   - Spawn de notas por timestamp.
   - Input window (Perfect/Good/Miss).
   - Score/multiplicador.
5. Conecta gameplay a Wwise RTPC/States.
6. Perf pass temprano (profiling de CPU, GC, voces de audio).

## Principios técnicos clave

- **Data-driven:** charts y metadata en JSON/ScriptableObjects, no hardcoded.
- **Determinismo musical:** usa DSP/song time como fuente de verdad para sincronía.
- **Bajo acoplamiento:** gameplay nunca debe depender de implementación concreta de audio.
- **Performance first:** object pooling para notas, cero allocations por frame crítico.

## Entregables de esta base

- Arquitectura modular para escalar de 1 canción a catálogo.
- Guía de integración Wwise con naming convention.
- Formato de chart versionado para compatibilidad futura.
- Roadmap por fases (0.1 → 0.3).

## Siguiente acción recomendada

Implementar un prototipo de escena única (`SongPlayScene`) con timing lane + input + scoring y validarlo con una canción corta (60–90 segundos).
