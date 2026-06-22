# 📋 GUÍA DE CONFIGURACIÓN EN INSPECTOR - JAM Platformer

## 🎯 PASO A PASO PARA APLICAR MEJORAS

### PASO 1: Verificar Animator (MC.controller)
**Ubicación**: Assets/personaje/animaciones/MC.controller

1. Abrir el Animator Controller haciendo doble-click
2. En el panel de Parámetros (arriba-izquierda), verificar/agregar:
   - ✅ `isGrounded` (bool) - Debe existir
   - ✅ `Speed` (float) - Debe existir
   - ⏳ `Jump` (trigger) - **AGREGAR SI NO EXISTE**
   - ⏳ `Land` (trigger) - **AGREGAR SI NO EXISTE**

3. Verificar transiciones (flechas entre estados):
   - `Idle` → `Walk`: Condición `Speed > 0.1`
   - `Walk` → `Idle`: Condición `Speed < 0.1`
   - `Walk/Idle` → `Jump`: Condición `Jump` (trigger)
   - `Jump` → `Fall` (automático o `isGrounded == false`)
   - `Fall` → `Land`: Condición `Land` (trigger)

---

### PASO 2: Configurar ThirdPersonController (MC)
**Ubicación**: Jerarquía > MC > Inspector

#### Sección "Movimiento"
```
moveSpeed = 10f                    (actual)
acceleration = 50f                 (actual)
airControl = 0.5f                  (CAMBIAR de 0.4)
rotationTime = 0.15f               (actual)
modelYawOffset = -90f              (actual)
```

#### Sección "Salto"
```
jumpForce = 12f                    (actual)
groundCheck = [Arrastrar objeto groundCheck]
groundDistance = 0.6f              (actual)
groundMask = [Layer: Ground]
```

#### Sección "Sonidos"
```
jumpUpClip = Assets/Sounds/JumpUp.mp3 ✅
jumpLandingClip = Assets/Sounds/JumpLanding.mp3 ✅
boingClip = Assets/Sounds/Boing.mp3 ✅
footstepClips = [ARRAY - CREAR]    ⏳
  - Tamaño: 2-3
  - Elemento 0: [Clip de paso 1]
  - Elemento 1: [Clip de paso 2]
footstepVolume = 0.5f              (nuevo)
```

#### Sección "Animaciones"
```
landingAnimDuration = 0.3f         (nuevo)
```

---

### PASO 3: Configurar PlayerRespawn (MC)
**Ubicación**: Jerarquía > MC > Inspector (scroll down)

```
killY = -15f                       (actual)
fallBelowCheckpoint = 60f          (actual)
respawnClip = [CREAR/BUSCAR]       ⏳
checkpointClip = [CREAR/BUSCAR]    ⏳
shakeAmount = 0.12f                (actual)
shakeDuration = 0.2f               (actual)
respawnPauseDuration = 0.1f        (nuevo)
```

---

### PASO 4: Configurar AnimationController (MC)
**Ubicación**: Jerarquía > MC > Inspector > Agregar Componente

1. Hacer click en "Add Component"
2. Buscar y agregar "AnimationController"
3. Configurar:
```
transitionDuration = 0.15f
landingAnimDuration = 0.2f
```

---

### PASO 5: Configurar PlayerMovementOptimizer (MC)
**Ubicación**: Jerarquía > MC > Inspector > Agregar Componente

1. Hacer click en "Add Component"
2. Buscar y agregar "PlayerMovementOptimizer"
3. Configurar:
```
Optimizaciones de Movimiento
  coyoteTime = 0.1f           (permite saltar 0.1s después de dejar plataforma)
  jumpBuffer = 0.05f          (permite presionar salto 0.05s antes de aterrizar)

Feedback Visual
  jumpScaleBoost = 1.05f      (expansión al saltar: 5%)
  jumpScaleDuration = 0.1f    (duración del efecto en segundos)
```

---

### PASO 6: Configurar Plataformas Rebotadoras
**Ubicación**: Jerarquía > Buscar plataformas rojas > Inspector

1. Seleccionar una plataforma roja (bouncy)
2. En el componente "BouncyPlatform":
```
boingClip = Assets/Sounds/Boing.mp3 ✅
boingVolume = 0.8f
bouncePower = 3.5f             (multiplicador de salto: 3.5x jumpForce)
bounceScale = 1.1f             (escala visual al rebote)
bounceDuration = 0.15f
```

3. **Repetir para TODAS las plataformas rebotadoras (rojas)**

---

### PASO 7: Verificar ThirdPersonCamera
**Ubicación**: Jerarquía > Main Camera > Inspector

Verificar que existe:
```
[Debe tener componente ThirdPersonCamera]
- Verificar que apunta al MC correcto
- Verificar distancia de cámara
```

---

## 🎵 AUDIO ASSETS FALTANTES

Para audio completamente funcional, necesitas:

### Clips Requeridos
1. **footstepClips** (2-3 variaciones):
   - Opción A: Crear con DAW (Audacity, REAPER)
   - Opción B: Buscar en:
     - Freesound.org (buscar "footstep", "step", "walk")
     - OpenGameArt.org
     - Kenney Assets (kenney.nl)

2. **checkpointClip** (sonido distintivo):
   - Algo tipo "ding", "chime", "success"
   - Duración: 0.3-0.8s

3. **respawnClip** (sonido de respawn):
   - Algo tipo "pop", "poof", "respawn"
   - Duración: 0.3-0.8s

### Importar Audio Clips en Unity
1. Crear carpeta `Assets/Sounds/` si no existe
2. Arrastrar archivos de audio (.mp3, .wav, .ogg)
3. Seleccionar el clip en Inspector:
   - Import Type: Default
   - Load Type: Decompress On Load (para efectos cortos)
   - Channels: Mono (para pasos, efectos)

---

## ✅ CHECKLIST DE FINALIZACIÓN

- [ ] Animator tiene triggers `Jump` y `Land`
- [ ] `airControl` = 0.5 en ThirdPersonController
- [ ] Array `footstepClips` asignado con clips
- [ ] `checkpointClip` asignado en PlayerRespawn
- [ ] `respawnClip` asignado en PlayerRespawn
- [ ] AnimationController agregado a MC
- [ ] PlayerMovementOptimizer agregado a MC
- [ ] BouncyPlatform configurado en plataformas rojas
- [ ] Todos los audio clips tienen volumen audible

---

## 🧪 TESTEAR CAMBIOS

1. **Entrar a Play Mode** (Ctrl+P o botón Play)
2. **Verificar**:
   - ✅ Pasos se escuchan al caminar
   - ✅ Sonido de salto se reproduce
   - ✅ Aterrizaje suena diferente a caída
   - ✅ Plataformas rebotadoras hacen "boing"
   - ✅ Respawn tiene sonido y animación
   - ✅ Checkpoints suenan distintivo
   - ✅ Control aéreo mejorado (más responsivo)

3. **Ajustar Parámetros** según preferencia:
   - Volúmenes demasiado fuertes → reducir
   - Saltos se sienten débiles → aumentar `jumpForce`
   - Demasiada fricción en aire → aumentar `airControl`

---

## 🐛 TROUBLESHOOTING

### "No se escuchan pasos"
- ✅ Verificar que `footstepClips` tiene elementos asignados
- ✅ Verificar volumen del juego (slider inferior)
- ✅ Verificar que `footstepVolume` > 0.1

### "Animación de salto no funciona"
- ✅ Verificar que Animator tiene triggers `Jump` y `Land`
- ✅ Verificar transiciones en Animator
- ✅ Verificar que ThirdPersonController está llamando `animator.SetTrigger("Jump")`

### "Plataformas rebotadoras no funcionan"
- ✅ Verificar que `BouncyPlatform` está en el script
- ✅ Verificar que la plataforma tiene `Rigidbody` con `isKinematic = true`
- ✅ Verificar que el jugador tiene collider

### "Respawn sin feedback"
- ✅ Verificar que `respawnClip` está asignado
- ✅ Verificar `PlayerRespawn` component existe en MC
- ✅ Revisar console por errores

---

## 📞 NOTAS IMPORTANTES

1. **Audio 2D vs 3D**: Los efectos de jugador están configurados como 2D (no hay panning)
   - Para cambiar a 3D, modificar `audioSource.spatialBlend = 1f;` en ThirdPersonController

2. **Compilación de Scripts**: Los scripts nuevos se compilarán automáticamente
   - Si hay errores, revisar Console en Unity

3. **Performance**: Los sonidos de pasos pueden impactar si hay muchos
   - Mantener Array pequeño (2-3 clips máximo)

4. **Futuras Mejoras**: Considerar en próxima iteración
   - Sistema de música de fondo
   - SFX de UI
   - Efectos de partículas

---

**¡Listo! Sigue esta guía paso-a-paso y tu juego tendrá feedback sensorial completo.** 🎮
