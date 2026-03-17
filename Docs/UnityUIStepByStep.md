# UI básica (HUD) - paso a paso en Unity 6

Esta guía te deja una HUD mínima para validar gameplay en cancha: score, combo, multiplicador, último hit y tiempo de canción.

## 1) Scripts usados

- `Assets/UI/Runtime/SongHudController.cs`
- `Assets/Gameplay/Runtime/Session/SongSessionController.cs` (expone datos para UI)

## 2) Crear Canvas y layout básico

1. En `SongPlayScene`, crea `GameObject > UI > Canvas`.
2. Render Mode: `Screen Space - Overlay`.
3. Crea un `Panel` dentro del Canvas (ancla arriba-izquierda).
4. Dentro del Panel crea 5 textos (`UI > Text`):
   - `ScoreText`
   - `ComboText`
   - `MultiplierText`
   - `HitQualityText`
   - `SongTimeText`

> Si usas TextMeshPro, puedes usarlo visualmente igual, pero este script base está hecho con `UnityEngine.UI.Text` para minimizar dependencias.

## 3) Agregar controlador de HUD

1. Crea objeto vacío `HUDController`.
2. Añade componente `SongHudController`.
3. Asigna referencias:
   - `Session Controller` = objeto con `SongSessionController`.
   - `Score Text` = `ScoreText`.
   - `Combo Text` = `ComboText`.
   - `Multiplier Text` = `MultiplierText`.
   - `Hit Quality Text` = `HitQualityText`.
   - `Song Time Text` = `SongTimeText`.
4. `Refresh Rate Hz` recomendado: 20.

## 4) Prueba rápida

1. Entra a Play Mode.
2. Presiona carriles (`A/S/D/F/G` o tus Input Actions).
3. Verifica que cambian:
   - Score
   - Combo
   - Multiplicador
   - Last Hit (Perfect/Good/Miss)
   - Song Time

## 5) Ajustes recomendados

- Usa fuente monoespaciada para legibilidad.
- Limita ancho del panel para no tapar lanes.
- Cuando tengas estilo final, migra de `Text` a TMP con un wrapper para no tocar lógica de HUD.
