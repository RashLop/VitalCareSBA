# Migración a Clean Architecture clásica

## Leyenda

<span style="color:#6f42c1"><b>■ Morado</b></span> = Entidades  
<span style="color:#0969da"><b>■ Azul</b></span> = Casos de uso  
<span style="color:#1a7f37"><b>■ Verde</b></span> = Adaptadores de interfaz  
<span style="color:#bc4c00"><b>■ Naranja</b></span> = Frameworks y drivers  

---

## Equivalencias exactas

| Archivo original | Nuevo archivo en Clean |
|---|---|
| `Application/Interfaces/IMedicamentoService.cs` | <span style="color:#0969da"><b>CasosDeUso/PuertosEntrada/IMedicamentoInputPort.cs</b></span> |
| `Application/Services/MedicamentoService.cs` | <span style="color:#0969da"><b>CasosDeUso/Interactores/MedicamentoInteractor.cs</b></span> |
| `Domain/Model/Medicamento/Medicamento.cs` | <span style="color:#6f42c1"><b>Entidades/Medicamento.cs</b></span> |
| `Domain/DTOs/MedicamentoDTO.cs` | <span style="color:#1a7f37"><b>AdaptadoresDeInterfaz/DTOs/MedicamentoDTO.cs</b></span> |
| `Domain/Ports/Output/IMedicamentoRepository.cs` | <span style="color:#1a7f37"><b>AdaptadoresDeInterfaz/Gateways/IMedicamentoRepository.cs</b></span> |
| `Domain/Ports/Output/IRepository.cs` | <span style="color:#1a7f37"><b>AdaptadoresDeInterfaz/Gateways/IRepository.cs</b></span> |
| `Domain/Validators/MedicamentoValidacion.cs` | <span style="color:#0969da"><b>CasosDeUso/Validadores/MedicamentoValidacion.cs</b></span> |
| `Domain/Validators/Result.cs` | <span style="color:#0969da"><b>CasosDeUso/Validadores/Result.cs</b></span> |
| `Application/Interfaces/IResult.cs` | <span style="color:#0969da"><b>CasosDeUso/Validadores/IResult.cs</b></span> |
| `Infrastructure/Repositories/MedicamentoRepository.cs` | <span style="color:#bc4c00"><b>FrameworksYDrivers/Repositorios/MedicamentoRepository.cs</b></span> |
| `Infrastructure/Creadores/MedicamentoRepositoryCreator.cs` | <span style="color:#bc4c00"><b>FrameworksYDrivers/Creadores/MedicamentoRepositoryCreator.cs</b></span> |
| `Infrastructure/Creadores/RepositoryCreator.cs` | <span style="color:#bc4c00"><b>FrameworksYDrivers/Creadores/RepositoryCreator.cs</b></span> |
| `Infrastructure/Persistence/Connection/ConexionStringSingleton.cs` | <span style="color:#bc4c00"><b>FrameworksYDrivers/Persistencia/Conexion/ConexionStringSingleton.cs</b></span> |
| `Infrastructure/Helpers/StringHelper.cs` | <span style="color:#bc4c00"><b>FrameworksYDrivers/Ayudadores/StringHelper.cs</b></span> |
| `Infrastructure/Helpers/RepositoryDbHelper.cs` | <span style="color:#bc4c00"><b>FrameworksYDrivers/Ayudadores/RepositoryDbHelper.cs</b></span> |
| `Infrastructure/Helpers/PasswordHelper.cs` | <span style="color:#bc4c00"><b>FrameworksYDrivers/Ayudadores/PasswordHelper.cs</b></span> |
| `Infrastructure/Helpers/CredencialesHelper.cs` | <span style="color:#bc4c00"><b>FrameworksYDrivers/Ayudadores/CredencialesHelper.cs</b></span> |
| `Controllers/MedicamentoController.cs` | <span style="color:#1a7f37"><b>AdaptadoresDeInterfaz/Controladores/MedicamentoController.cs</b></span> |
| No existe en el original | <span style="color:#1a7f37"><b>AdaptadoresDeInterfaz/Presentadores/MedicamentoPresenter.cs</b></span> |

---

## Estructura final

```txt
Servicio1Clean
│
├── Entidades
│   └── Medicamento.cs
│
├── CasosDeUso
│   ├── PuertosEntrada
│   │   └── IMedicamentoInputPort.cs
│   ├── PuertosSalida
│   │   └── IMedicamentoOutputPort.cs
│   ├── Interactores
│   │   └── MedicamentoInteractor.cs
│   └── Validadores
│       ├── MedicamentoValidacion.cs
│       ├── Result.cs
│       └── IResult.cs
│
├── AdaptadoresDeInterfaz
│   ├── Gateways
│   │   ├── IRepository.cs
│   │   └── IMedicamentoRepository.cs
│   ├── DTOs
│   │   └── MedicamentoDTO.cs
│   ├── Presentadores
│   │   └── MedicamentoPresenter.cs
│   └── Controladores
│       └── MedicamentoController.cs
│
└── FrameworksYDrivers
    ├── Persistencia
    │   └── Conexion
    │       └── ConexionStringSingleton.cs
    ├── Repositorios
    │   └── MedicamentoRepository.cs
    ├── Creadores
    │   ├── RepositoryCreator.cs
    │   └── MedicamentoRepositoryCreator.cs
    └── Ayudadores
        ├── StringHelper.cs
        ├── RepositoryDbHelper.cs
        ├── PasswordHelper.cs
        └── CredencialesHelper.cs