# Sistema de Inventario en C#

# Arquitectura del proyecto

```text
SistemaDeInventario/
│
├── Models/
├── Services/
├── Data/
├── UI/
├── data/
│
├── Program.cs
├── README.md
└── SistemaDeInventario.csproj
```
# Descripción de capas
## Models

Representan las entidades del sistema:

- Producto
- Venta
- Movimiento

Estas clases contienen únicamente datos del dominio.

## Services

Contienen la lógica del negocio:

- Registrar productos
- Agregar stock
- Registrar ventas
- Eliminar productos
- Buscar productos
- Registrar movimientos
## Data

Responsable de la persistencia en disco utilizando JSON.

La clase JsonDataManager centraliza:

- Lectura de archivos
- Escritura de archivos
- Serialización
- Deserialización
- Manejo de rutas
## UI

Contiene toda la interacción con consola:

- Menús
- Lectura de datos
- Mensajes al usuario

# Cómo ejecutar el proyecto

## Requisitos

- .NET SDK instalado

Verificar instalación:

```bash
dotnet --version
```

## Ejecutar

Desde la raíz del proyecto:
```
dotnet run
```
## Instalación de dependencias

Instalar Newtonsoft.Json:
```
dotnet add package Newtonsoft.Json
```
