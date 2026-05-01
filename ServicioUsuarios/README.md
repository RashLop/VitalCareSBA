# Guía de migración a Arquitectura Hexagonal

Esta guía define cómo reorganizar el servicio usando Arquitectura Hexagonal, respetando la estructura original del proyecto y usando `Medicamento` como ejemplo.

---

## Guía de colores

| Color | Capa | Responsabilidad |
|---|---|---|
| <span style="color:#6f42c1"><b>■ Morado</b></span> | `Dominio` | Modelos, DTOs, validadores y puertos |
| <span style="color:#0969da"><b>■ Azul</b></span> | `Aplicacion` | Servicios de aplicación y fachadas |
| <span style="color:#1a7f37"><b>■ Verde</b></span> | `Adaptadores` | Controladores HTTP y adaptadores de entrada |
| <span style="color:#bc4c00"><b>■ Naranja</b></span> | `Infraestructura` | Repositorios, conexión, ayudadores y creadores |

---

## 1. Estructura final recomendada

```txt
ServicioMedicamentos
│
├── 🟣 Dominio
│   ├── 🟣 DTOs
│   │   └── MedicamentoDTO.cs
│   │
│   ├── 🟣 Modelo
│   │   └── Medicamento
│   │       └── Medicamento.cs
│   │
│   ├── 🟣 Puertos
│   │   ├── Entrada
│   │   │   └── IMedicamentoService.cs
│   │   │
│   │   └── Salida
│   │       ├── IRepository.cs
│   │       └── IMedicamentoRepository.cs
│   │
│   └── 🟣 Validadores
│       ├── MedicamentoValidacion.cs
│       ├── Result.cs
│       └── IResult.cs
│
├── 🔵 Aplicacion
│   ├── 🔵 Fachadas
│   │   └── MedicamentoFacade.cs
│   │
│   └── 🔵 Servicios
│       └── MedicamentoService.cs
│
├── 🟢 Adaptadores
│   └── 🟢 Entrada
│       └── 🟢 Controladores
│           └── MedicamentoController.cs
│
├── 🟠 Infraestructura
│   ├── 🟠 Creadores
│   │   ├── RepositoryCreator.cs
│   │   └── MedicamentoRepositoryCreator.cs
│   │
│   ├── 🟠 Ayudadores
│   │   ├── StringHelper.cs
│   │   ├── RepositoryDbHelper.cs
│   │   ├── PasswordHelper.cs
│   │   └── CredencialesHelper.cs
│   │
│   ├── 🟠 Persistencia
│   │   └── Conexion
│   │       └── ConexionStringSingleton.cs
│   │
│   └── 🟠 Repositorios
│       └── MedicamentoRepository.cs
│
├── Program.cs
├── appsettings.json
└── appsettings.Development.json