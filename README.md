# 🧾 Proyecto: Panel de Análisis de Empleados KPMG

**Tecnologías:** ASP.NET Core 8 · Entity Framework Core · SQL Server · Razor · Chart.js · Bootstrap 5 · ClosedXML · JQuery . AJAX · JavaScript · HTML · CSS

## Descripción general

El sistema es una aplicación web desarrollada con ASP.NET Core 8 que permite visualizar, gestionar y analizar la información de empleados de una organización.  

Incluye funcionalidades de autenticación, CRUD de empleados, panel de análisis con gráficas interactivas y exportación de reportes para áreas de recursos humanos.

---

## Estructura de la solución

```
├── SL/                    → Proyecto Web API
│   ├── Controllers/
|   |   └── ChartsController.cs
|   |   └── LoginController.cs
│   │   └── EmployeeController.cs
│   └── Program.cs
│
├── BL/                     → Capa de lógica de negocio (llama SP y consultas LINQ)
│
├── DL/                     → Capa de datos (Entity Framework, DTOs, context)
│   └── PruebaTecnicaContext.cs
│
├── ML/                     → Capa de Modelo (clases como: Employee, Report, etc.)
│
├── PL/                    → Proyecto MVC con Razor
│   ├── Controllers/
│   │   └── BenchedController.cs
|   |   └── ChartController.cs
│   │   └── EmployeeController.cs
│   │   └── LoginController.cs
│   ├── Views/
│   │   └── Benched/
|   |   └── Chart/
│   │   └── Employee/
│   │   └── Login/
│   └── wwwroot/
│
└── README.md
```

---

## ⚙️ Instalación y configuración

### 🧩 Requisitos previos

- .NET Core 8 
- SQL Server 2012 o superior  
- Visual Studio 2022 
- Navegador Edge, Chrome o Firefox

### 🧩 Pasos de instalación

1. Clonar el repositorio:
   ```
   git clone https://github.com/MichelleDigiS01/PruebaTecEmployee.git
   ```

2. Ejecutar el script llamado `PruebaTecnicaKPMG.sql` en SQL Server


3. Configurar la cadena de conexión en `appsettings.json` de la API:
   ```json
   "ConnectionStrings": {
     "TalentAnalytics": "Server=.;Database=PruebaTecnica;User Id=sa;Password=pass@word1;TrustServerCertificate=True"
   }
   ```

4. Ejecutar ambos proyectos (API y MVC) desde Visual Studio:

---

## 🧠 Funcionalidades implementadas

### 🔐 Autenticación y Roles
- Inicio de sesión mediante JWT Tokens.  
- Roles disponibles: **RRHH**, **Gerente**, **Analista**.  
- Control de acceso por rol a vistas específicas.

### 👩‍💼 CRUD de Empleados
- Alta, consulta, edición y eliminación de registros.  
- Búsqueda avanzada con filtros (género, educación, ciudad, experiencia, etc.). 

### 📊 Panel de Análisis
- Visualización de métricas y tendencias mediante **Chart.js**:  
  - Distribución por **Género**  
  - Distribución por **Ciudad**  
  - Correlación **Experiencia vs Nivel de Pago**  
  - Tasa de **Abandono (LeaveOrNot)**  

### 📤 Exportación de Reportes
- Botones de exportación a CSV desde el dashboard.  
- Reportes disponibles:
  - Diversidad (género, ciudad, educación)  
  - Rotación (abandono)  
  - Talento (experiencia vs nivel de pago)

### 📱 Diseño responsivo
- Implementado con **Bootstrap 5**.  
- Compatible con dispositivos móviles, tablet y escritorio.

---

## 🧩 Endpoints principales (API)

| Método | Endpoint | Descripción |
|--------|-----------|-------------|
| `GET`  | `/api/Charts/gender` | Distribución por género |
| `GET`  | `/api/Charts/city` | Distribución por ciudad |
| `GET`  | `/api/Charts/education` | Distribución por educación |
| `GET`  | `/api/Charts/averageAge` | Edad Promedio  |
| `GET`  | `/api/Charts/CorrelationExperiencePayment` | Correlación experiencia / nivel de pago |
| `GET`  | `/api/Charts/leavePediction` | Prediccion de Abandono |
| `GET`  | `/api/Employee/GetAll` | Muestra todos los Employees |
| `GET`  | `/api/Employee/GetById` | Muestra Employee por Id |
| `POST` | `/api/Employee/Add` | Agregar un nuevo Employee |
| `PUT`  | `/api/Employee/Update` | Actualiza información de Employee|
|`DELETE`| `/api/Employee/Delete` | Elimina un Employee |
| `GET` | `/api/Employee/benched` | Employee que estuvieron benched |
| `GET` | `/api/Login/GetUserByEmail` | Trae usuario por Email |

---

## 🧑‍💻 Autor

**Michelle Zoé Palomino Gómez**  
📧 michellezoe01@hotmail.com  

---

