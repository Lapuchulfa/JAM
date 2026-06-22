# ⚙️ CONFIGURACIÓN PASO A PASO - JAM Platformer

**Tiempo estimado**: 20-30 minutos  
**Dificultad**: Media  
**Status**: Los scripts ya están compilados ✅

---

## 🎯 OBJETIVO
Configurar Unity Inspector para que todos los componentes trabajen juntos correctamente.

---

## PASO 1️⃣: Seleccionar el Personaje (MC)

### En la **Jerarquía** (panel izquierdo):
1. Busca el objeto `MC` (está expandido en el árbol)
2. **Haz click en `MC`**

**Resultado esperado**: El Inspector (derecha) debe mostrar los componentes de MC

---

## PASO 2️⃣: Verificar ThirdPersonController

En el **Inspector (panel derecho)**, busca el componente `ThirdPersonController`

### Verificar/Cambiar valores:

```
📍 SECCIÓN "Movimiento"
  ✅ moveSpeed = 10
  ✅ acceleration = 50
  🔧 airControl = [VERIFICAR QUE SEA 0.5]
     └─ Si es 0.4, CAMBIARLO a 0.5
  ✅ rotationTime = 0.15
  ✅ modelYawOffset = -90
```

### Asignar Audio Clips:
```
📍 SECCIÓN "Sonidos"
  
  jumpUpClip = [Ver abajo]
  jumpLandingClip = [Ver abajo]
  boingClip = [Ver abajo]
  footstepClips = [Ver abajo]
  footstepVolume = 0.5 ✅
```

#### ❗ IMPORTANTE: Cómo asignar audio clips:

1. **Para cada campo de audio** (jumpUpClip, jumpLandingClip, boingClip):
   - Haz click en el pequeño círculo "⊙" al lado del campo
   - Se abrirá una ventana de selección
   - Busca en `Assets/Sounds/`
   - Selecciona el clip correspondiente:
     - `jumpUpClip` → `JumpUp.mp3`
     - `jumpLandingClip` → `JumpLanding.mp3`
     - `boingClip` → `Boing.mp3`

2. **Para footstepClips** (es un Array):
   - Busca el campo `Footstep Clips` (Size: 0)
   - Haz click en "Size"
   - Cambia a `Size: 2`
   - En `Element 0`: Asigna `JumpUp.mp3` (placeholder)
   - En `Element 1`: Asigna `JumpLanding.mp3` (placeholder)

---

## PASO 3️⃣: Verificar PlayerRespawn

En el **Inspector**, busca el componente `PlayerRespawn` (scroll down si es necesario)

```
📍 SECCIÓN "Limites"
  ✅ killY = -15
  ✅ fallBelowCheckpoint = 60

📍 SECCIÓN "Juiciness"
  respawnClip = [ASIGNAR OPCIONAL]
  checkpointClip = [ASIGNAR OPCIONAL]
  ✅ shakeAmount = 0.12
  ✅ shakeDuration = 0.2
  respawnPauseDuration = 0.1 ✅
```

**NOTA**: Para respawnClip y checkpointClip, puedes usar placeholders o dejarlos vacíos por ahora

---

## PASO 4️⃣: Verificar AnimationController

En el **Inspector** de MC, busca el componente `AnimationController`

Si **NO EXISTE**, debemos agregarlo:
1. Haz click en "Add Component" (abajo del Inspector)
2. Escribe "AnimationController"
3. Selecciónalo cuando aparezca

Valores:
```
📍 SECCIÓN "Estados de Animación"
  transitionDuration = 0.15 ✅
  landingDuration = 0.2 ✅
```

---

## PASO 5️⃣: Verificar PlayerMovementOptimizer

En el **Inspector** de MC, busca el componente `PlayerMovementOptimizer`

Si **NO EXISTE**, debemos agregarlo:
1. Haz click en "Add Component"
2. Escribe "PlayerMovementOptimizer"
3. Selecciónalo cuando aparezca

Valores:
```
📍 SECCIÓN "Optimizaciones de Movimiento"
  coyoteTime = 0.1 ✅
  jumpBuffer = 0.05 ✅

📍 SECCIÓN "Feedback Visual"
  jumpScaleBoost = 1.05 ✅
  jumpScaleDuration = 0.1 ✅
```

---

## PASO 6️⃣: CRÍTICO - Configurar Animator ⚠️

### Este es el paso MANUAL más importante

1. En la **Jerarquía**, selecciona `MC`
2. En el **Inspector**, busca el componente `Animator`
3. Busca el campo `Controller` 
4. Haz **doble-click** en el valor (debería ser `MC`)
5. Se abrirá el **Animator Controller** en una nueva ventana

### Una vez en la ventana del Animator:

#### A) Agregar Parámetros (arriba-izquierda):

1. Busca el panel "Parameters" (arriba a la izquierda)
2. Haz click en el **"+"** 
3. Selecciona **"Trigger"**
4. Nómbralo **`Jump`**
5. Repite: Haz click en **"+"** → **"Trigger"** → Nómbralo **`Land`**

**Resultado**: Debes tener 4 parámetros:
```
✅ isGrounded (bool)
✅ Speed (float)
✅ Jump (trigger) ← NUEVO
✅ Land (trigger) ← NUEVO
```

#### B) Configurar Transiciones (en el gráfico):

En la zona con los estados (Idle, Walk, Jump, etc.):

1. **Idle → Walk**
   - Condición: `Speed > 0.1`
   - ✅ Debe existir

2. **Walk → Idle**
   - Condición: `Speed < 0.1`
   - ✅ Debe existir

3. **Idle/Walk → Jump** (NUEVA)
   - Haz click derecho en Idle
   - "Make Transition"
   - Haz click en Jump
   - Selecciona la flecha
   - En Inspector: Conditions → **Trigger "Jump"**
   - Desactiva "Has Exit Time"

4. **Jump → Fall** (Debe existir automático)
   - Verificar que existe
   - Condición: `isGrounded == false`

5. **Fall → Land** (NUEVA si no existe)
   - Haz click derecho en Fall
   - "Make Transition"
   - Haz click en un estado Land (si existe)
   - O crea un estado Landing/Land si no existe
   - Selecciona la flecha
   - Conditions → **Trigger "Land"**
   - Desactiva "Has Exit Time"

6. **Land → Idle** (si existe estado Land)
   - Condición: `isGrounded == true`
   - Desactiva "Has Exit Time"

---

## PASO 7️⃣: Configurar Plataformas Rebotadoras

### Para CADA plataforma roja (rebotadora):

1. En la **Jerarquía**, busca una plataforma roja
2. Selecciónala
3. En el **Inspector**, busca `BouncyPlatform`
4. Asigna valores:

```
📍 SECCIÓN "Sonidos"
  boingClip = Boing.mp3 ✅

📍 SECCIÓN "Mecánica"
  boingVolume = 0.8
  bouncePower = 3.5
  bounceScale = 1.1
  bounceDuration = 0.15
```

⚠️ **IMPORTANTE**: Debes hacer esto para TODAS las plataformas rojas que veas en la jerarquía

---

## PASO 8️⃣: Verificar Componentes Agregados

En el **Inspector** de MC, scroll hacia abajo y verifica que existan:
- ✅ `ThirdPersonController`
- ✅ `PlayerRespawn`
- ✅ `AnimationController` ← Debe estar agregado
- ✅ `PlayerMovementOptimizer` ← Debe estar agregado
- ✅ `Animator`
- ✅ `Rigidbody`
- ✅ `Collider`

Si falta alguno, haz click en "Add Component" y búscalo

---

## ✅ VERIFICACIÓN FINAL

Marca cada ítem conforme lo completes:

### Componentes
- [ ] ThirdPersonController - airControl = 0.5
- [ ] ThirdPersonController - Audio clips asignados (3)
- [ ] ThirdPersonController - footstepClips array = 2 elementos
- [ ] PlayerRespawn - Valores por defecto OK
- [ ] AnimationController - Agregado y configurado
- [ ] PlayerMovementOptimizer - Agregado y configurado

### Animator
- [ ] Trigger "Jump" creado
- [ ] Trigger "Land" creado
- [ ] Transición Idle/Walk → Jump configurada
- [ ] Transición Fall → Land configurada
- [ ] Transición Land → Idle configurada

### Plataformas
- [ ] TODAS las plataformas rojas tienen BouncyPlatform configurado

---

## 🧪 TESTEAR

Una vez completado todo:

1. **Haz click en PLAY** (botón arriba en el toolbar)
2. En la escena del juego, intenta:
   - ✅ Caminar (deberías escuchar pasos)
   - ✅ Saltar (sonido de salto + animación)
   - ✅ Aterrizar (sonido diferente al salto)
   - ✅ Ir a plataforma roja (sonido "boing")
   - ✅ Caer fuera (respawn con feedback)

3. **Haz click en STOP** para terminar Play

---

## 🐛 SI ALGO FALLA

### "No se escuchan pasos"
→ Verificar que `footstepClips` tiene elementos asignados
→ Verificar que `footstepVolume > 0.1`

### "Animación de salto no funciona"
→ Verificar que Animator tiene triggers `Jump` y `Land`
→ Verificar transiciones en Animator (ver arriba)

### "Error en Console"
→ Abrir "Window" → "General" → "Console"
→ Ver mensajes de error
→ Compartir error conmigo

### "Plataformas rojas no rebotan"
→ Verificar que cada una tiene componente `BouncyPlatform`
→ Verificar que `Rigidbody` tiene `isKinematic = true`

---

## 📞 PREGUNTAS FRECUENTES

**P: ¿Cuánto tiempo toma?**  
R: 20-30 minutos si es tu primera vez. Más rápido después.

**P: ¿Puedo dejar campos vacíos?**  
R: Los campos de audio pueden quedar vacíos pero la experiencia no será completa.

**P: ¿Pierdo datos si algo falla?**  
R: No, siempre puedes volver atrás con Ctrl+Z

**P: ¿Cómo cambio sonidos después?**  
R: Inspector → Campo de audio → Click "⊙" → Seleccionar nuevo clip

---

## 📚 REFERENCIAS

- Detalles técnicos: `FEEDBACK_VISUAL_Y_GAMEPLAY.md`
- Guía alternativa: `SETUP_INSPECTOR_GUIDE.md`
- Cambios de código: Revisar commits git

---

**¡Listo! Sigue este guía paso-a-paso y todo debería funcionar.** 🎮

Si tienes dudas o errores, abre la **Console** (Window → General → Console) y comparte los mensajes.
