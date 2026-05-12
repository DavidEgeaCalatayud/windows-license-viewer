# Windows License Viewer

Windows License Viewer es una aplicación ligera de escritorio para Windows orientada a tareas de soporte técnico, inventario y documentación administrativa de equipos.

Permite consultar de forma visual información relacionada con la licencia de Windows y datos identificativos del equipo, como el número de serie BIOS o el UUID del dispositivo.

## Contexto del proyecto

Este proyecto fue desarrollado para cubrir una necesidad real en un entorno empresarial: facilitar la recopilación de información de licencia y número de serie de equipos Windows durante procesos de inventario y justificación administrativa.

La herramienta está pensada para que una persona sin conocimientos técnicos pueda obtener esta información de forma rápida, clara y visual, sin necesidad de ejecutar comandos, acceder al registro de Windows o utilizar herramientas avanzadas del sistema.

El objetivo principal es reducir errores manuales, agilizar la recogida de datos y facilitar la obtención de evidencias visuales necesarias en procesos administrativos relacionados con equipamiento informático, como auditorías internas, inventarios o justificaciones vinculadas a soluciones de puesto de trabajo seguro.

## Características

- Consulta visual de información de licencia de Windows.
- Obtención del número de serie del equipo mediante WMI.
- Consulta de UUID del dispositivo como identificador alternativo.
- Interfaz sencilla orientada a usuarios no técnicos.
- Funcionamiento local, sin conexión a servidores externos.
- Utilidad práctica para soporte, inventario y documentación administrativa.

## Tecnologías utilizadas

- C#
- .NET 8
- Windows Forms
- Windows Registry
- WMI / System.Management

## Privacidad

La aplicación se ejecuta de forma local en el equipo.

No envía información a servidores externos, no almacena datos de licencia y no realiza conexiones de red.

Se recomienda no compartir capturas de pantalla que contengan claves, números de serie o identificadores reales del dispositivo.

## Uso responsable

Esta herramienta está pensada exclusivamente para equipos propios o entornos donde se cuenta con autorización expresa.

No debe utilizarse para consultar información de sistemas ajenos sin permiso.

## Limitaciones

En equipos con activación digital, licencias OEM, KMS o licencias por volumen, la clave mostrada puede no corresponder a una clave reutilizable real.

La herramienta debe entenderse como apoyo para tareas de soporte, inventario y documentación, no como sistema oficial de auditoría de licencias.

## Estado del proyecto

Proyecto pequeño desarrollado para resolver una necesidad concreta de soporte técnico e inventario en entorno empresarial.
