# Aplicación de Realidad Aumentada con Narrativa Interactiva

## Unity + Vuforia

---

## Descripción

Este repositorio contiene el desarrollo progresivo de una aplicación de **realidad aumentada (AR)** implementada en Unity utilizando el motor **Vuforia**.

El proyecto se construye a través de cinco prácticas, evolucionando desde la configuración inicial del entorno hasta la implementación de una **experiencia interactiva con narrativa**, basada en múltiples marcadores (Image Targets) y minijuegos.

El objetivo principal es demostrar la integración de:

* Reconocimiento de imágenes en AR
* Interacción mediante interfaz gráfica
* Control de lógica en C#
* Diseño de experiencias narrativas interactivas

---

## Prácticas desarrolladas

### Práctica 1: Configuración inicial

![Practica1](gifs/practica1.gif)

Configuración del entorno de desarrollo en Unity, integración de Vuforia y despliegue de un modelo 3D sobre un Image Target.

---

### Práctica 2: Interfaz gráfica

![Practica2](gifs/practica2.gif)

Implementación de un sistema de UI mediante Canvas, incluyendo botones para interactuar con el modelo (cambio de color).

---

### Práctica 3: Cambios dinámicos

![Practica3](gifs/practica3.gif)

Extensión del sistema de interacción con generación de colores aleatorios y manipulación de múltiples materiales del modelo.

---

### Práctica 4: Navegación y escenas

![Practica4](gifs/practica4.gif)

Implementación de un sistema modular de navegación entre escenas mediante un controlador general.

---

## Práctica 5: Aplicación con narrativa (Enfoque principal)

![Narrativa1](gifs/practica5_1.gif)
![Narrativa2](gifs/practica5_2.gif)
![Narrativa3](gifs/practica5_3.gif)
![Narrativa4](gifs/practica5_4.gif)
![Narrativa5](gifs/practica5_5.gif)

En esta práctica se desarrolla una experiencia completa de realidad aumentada con narrativa interactiva.

### Narrativa

El usuario acompaña a un personaje virtual tipo Mii a lo largo de su rutina diaria. Cada marcador representa una actividad distinta, generando una experiencia estructurada en eventos.

* Introducción narrativa con interacción inicial
* Secuencia de actividades (minijuegos)

---

### Sistemas elaborados

#### Movimiento del personaje

* Desplazamiento automático entre Image Targets
* Interpolación suave con:

  * `Vector3.Lerp` (posición)
  * `Quaternion.Slerp` (rotación)
* Orientación dinámica hacia el destino y la cámara

#### Control narrativo

* Sistema basado en eventos y contador de visitas
* Activación progresiva de actividades

#### Interfaz (UI)

* Implementada con Canvas
* Persistente incluso al perder tracking
* Mejora la experiencia de usuario en AR

#### Sistema de texto

* Efecto tipo máquina de escribir
* Presentación progresiva de mensajes narrativos

---

## Tecnologías utilizadas

* Unity 6000.3.8f1
* Vuforia Engine
* C#
* Blender (modelado 3D)
* Mixamo (animaciones)

---

## Instalación y ejecución

1. Clonar el repositorio:

2. Abrir el proyecto en Unity Hub (versión 6000.3.8f1)

3. Configurar Vuforia:

* Agregar licencia
* Importar base de datos de targets

4. Ejecutar en:

* Editor de Unity (modo cámara)
* Dispositivo móvil compatible

O descargar el apk en:
[Obtener APK](https://drive.google.com/drive/folders/1HdSzUIaHAjCCGWgiaCjchtQAhpZess14?usp=sharing)

---

## Uso

1. Apuntar la cámara a los marcadores (Image Targets)
2. Observar el despliegue del personaje
3. Interactuar con los minijuegos mediante la interfaz
4. Completar la secuencia narrativa

---

## Estado del proyecto

Proyecto académico finalizado.

---

## 👤 Autor

**Erick López González**
Facultad de Ingeniería, UNAM
Semestre 2026-2

---

## 📚 Notas

Este proyecto fue desarrollado con fines académicos como parte de prácticas de laboratorio enfocadas en el desarrollo de aplicaciones de realidad aumentada utilizando Unity y Vuforia.
