# Glosario del dominio — ES → EN

Donde la industria (Managed Print Services) ya tiene un término, se usa el de la
industria, no la traducción literal. Esto hace que el esquema mapee 1:1 cuando se integre con PaperCut, FMAudit o Ricoh @Remote.

---

## Activos

| Español (negocio)           | Inglés (código) | Nota                                                             |
| --------------------------- | --------------- | ---------------------------------------------------------------- |
| Equipo                      | `Asset`         | Cubre impresoras, laptops y lo que venga                         |
| Impresora                   | `Printer`       |                                                                  |
| Código de activo / etiqueta | `AssetTag`      | **Término de industria.** Identidad humana estable. NUNCA cambia |
| Número de serie             | `SerialNumber`  | Atributo mutable, NO identificador                               |
| Marca                       | `Brand`         |                                                                  |
| Modelo                      | `AssetModel`    | Aquí vive `IsColour`                                             |
| Bodega / Taller             | `Warehouse`     | Son la misma ubicación                                           |
| Ubicación                   | `Location`      | Warehouse \| Customer \| Disposed                                |
| Condición                   | `Condition`     | Ver abajo                                                        |
| Propiedad                   | `Ownership`     | Sps \| SoldToCustomer \| CustomerOwned                           |
| Dar de baja / desechar      | `WriteOff`      | Baja lógica. El equipo NUNCA se borra                            |
| Canibalizar                 | `Cannibalize`   |                                                                  |

### Condición

| Español    | Inglés          | Significado                                                        |
| ---------- | --------------- | ------------------------------------------------------------------ |
| Nueva      | `New`           | Nunca desplegada. No requiere taller                               |
| Habilitada | `Refurbished`   | Pasó por taller. Apta para instalar                                |
| Operativa  | `PendingRefurb` | Funciona, NO pasó por taller. ← el nombre en inglés sí dice qué es |
| Partes     | `ForParts`      | Donante de canibalización                                          |
| Chatarra   | `Scrap`         | Espera de desecho. Terminal                                        |

> Nota: una impresora `New` instalada en cliente **sigue siendo** `New`.
> Eso no es un hueco: te dice qué clientes tienen máquina nueva y cuáles usada.

## Contadores

| Español                 | Inglés                  | Nota                                        |
| ----------------------- | ----------------------- | ------------------------------------------- |
| Contador                | `Meter`                 | **Término de industria**                    |
| Lectura de contador     | `MeterReading`          | **Término de industria.** Append-only       |
| Contador blanco y negro | `MonoMeter`             |                                             |
| Contador a color        | `ColourMeter`           | `NULL` obligatorio si el modelo no es color |
| Ciclo de contador       | `MeterCycle`            | Resuelve el cambio de controlador           |
| Cambio de controlador   | `ControllerReplacement` | El contador puede subir **o bajar**         |
| Cambio de BICU          | `BicuReplacement`       |                                             |

## Contratos

| Español                          | Inglés                  | Nota                                                                |
| -------------------------------- | ----------------------- | ------------------------------------------------------------------- |
| Contrato                         | `Contract`              |                                                                     |
| Cliente                          | `Customer`              |                                                                     |
| Sucursal                         | `Branch`                | FEHIERRO = 1 Customer, N Branches                                   |
| Área                             | `Area`                  |                                                                     |
| Precio por página                | `ClickCharge`           | **Término de industria.** Vive en `ContractAsset`, no en `Contract` |
| Canon mínimo / páginas incluidas | `IncludedVolume`        | `NULL` = sin mínimo → marcar en rojo                                |
| Renta por página                 | `CostPerPage (CPP)`     | El modelo de negocio tiene nombre                                   |
| Canon fijo mensual               | `MonthlyFee`            | Para laptops y equipos sin contador                                 |
| Fecha de corte de datos          | `DataCutoffDate`        | Desde cuándo el histórico es confiable                              |
| Punto de equilibrio / retorno    | `Payback` / `BreakEven` |                                                                     |

## Inventario

| Español                  | Inglés                          | Nota                                   |
| ------------------------ | ------------------------------- | -------------------------------------- |
| Repuesto                 | `SparePart`                     |                                        |
| Suministro / tóner       | `Consumable`                    | **Término de industria**               |
| Mercadería para la venta | `Merchandise`                   |                                        |
| Kardex                   | `StockLedger` / `StockMovement` | Kardex es término local, no traduce    |
| Movimiento de stock      | `StockMovement`                 | Append-only. La verdad                 |
| Saldo                    | `StockBalance`                  | Proyección, no fuente de verdad        |
| Reservado / comprometido | `Reserved`                      | Aprobado pero no despachado            |
| Egreso                   | `Issue`                         | **Término de industria** (goods issue) |
| Solicitud de egreso      | `IssueRequest`                  |                                        |
| Despacho                 | `Dispatch`                      | El único paso que toca el stock        |
| Código de barras         | `Barcode`                       |                                        |
| Consumo por servicio     | `ServiceConsumption`            | Costo de SPS. NO se factura            |
| Venta                    | `Sale`                          | Se factura                             |
| Cortesía                 | `Courtesy`                      | Ver abajo                              |
| Ajuste de inventario     | `StockAdjustment`               |                                        |

### Tipos de cortesía

| Español                      | Inglés        | Dónde pega                                             |
| ---------------------------- | ------------- | ------------------------------------------------------ |
| Regalo para cerrar contrato  | `Acquisition` | **Inversión inicial** → sube el payback desde el mes 0 |
| Regalo de navidad al cliente | `Retention`   | **Costo del período** → baja el margen del mes         |
| Regalo a un prospecto        | `Prospecting` | Gasto. No imputable a contrato                         |

## Roles

| Español                         | Inglés                    |
| ------------------------------- | ------------------------- |
| Técnico                         | `Technician`              |
| Supervisor Técnico              | `TechSupervisor`          |
| Asistente de Supervisor Técnico | `AssistantTechSupervisor` |
| Gerente Técnico                 | `TechManager`             |
| Vendedor / postventa            | `Salesperson`             |
| Administrador                   | `Admin`                   |

> Recordatorio: el código chequea **permisos**, no roles.
> `TechSupervisor` y `AssistantTechSupervisor` despachan igual — si gestionamos roles,
> agregar un rol nuevo significa tocar 40 endpoints.

---

## Lo que va en español

**Solo la data.** Nombres de clientes, direcciones, nombres de sucursales,
descripciones de productos, motivos escritos por usuarios, observaciones de tickets.

Todo lo demás — carpetas, clases, propiedades, tablas, columnas, procedimientos
almacenados, ramas de git, mensajes de commit — en inglés.
