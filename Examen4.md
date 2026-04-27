# Examen 4. Reprogramación de vuelo y lista de espera

## Descripción

El proyecto base permite reservar, confirmar o cancelar, pero no contempla escenarios de alta demanda o cambios operativos. Este examen agrega dos capacidades nuevas: **reprogramación de reservas** y **lista de espera**.

## Objetivos

- Permitir al cliente cambiar su reserva a otro vuelo compatible.
- Implementar lista de espera para vuelos llenos.
- Reasignar automáticamente cupos liberados.
- Fortalecer reglas de negocio más realistas.

## Requerimiento a implementar

Agregar al sistema principal lo siguiente:

- Reprogramación de una reserva confirmada a otro vuelo.
- Validación de compatibilidad de ruta y fecha.
- Lista de espera cuando no haya asientos disponibles.
- Promoción automática desde lista de espera cuando se libere un asiento.
- Registro del historial de cambios de vuelo.

## Lógica de Negocio

### Flujo para reprogramar una reserva

- Mostrar opción en el menú: **Reprogramar reserva**.
- Solicitar el identificador de la reserva.
- Verificar que la reserva exista.
- Verificar que la reserva esté en estado **Confirmada**.
- Mostrar los datos actuales de la reserva:
  - cliente
  - vuelo actual
  - ruta
  - fecha
- Solicitar el nuevo vuelo al cual se desea cambiar la reserva.
- Verificar que el nuevo vuelo exista.
- Validar que el nuevo vuelo sea compatible:
  - mismo origen o ruta permitida
  - mismo destino o destino permitido
  - fecha válida según la regla del negocio
- Verificar que el nuevo vuelo no sea el mismo vuelo actual.
- Verificar disponibilidad de asientos en el nuevo vuelo.

### Si el nuevo vuelo tiene cupo disponible

- Liberar el asiento del vuelo anterior.
- Descontar un asiento del nuevo vuelo.
- Actualizar la reserva con el nuevo vuelo.
- Cambiar el estado de la reserva a **Reprogramada** o mantenerla como **Confirmada**, según la lógica definida.
- Registrar el cambio en `HistorialReprogramacion` con:
  - id de reserva
  - vuelo anterior
  - nuevo vuelo
  - fecha del cambio
  - motivo
- Guardar cambios en MySQL.
- Mostrar mensaje de éxito en consola.

### Si el nuevo vuelo no tiene cupo disponible

- Preguntar si el cliente desea entrar a la **lista de espera**.
- Verificar que no esté ya registrado en espera para ese vuelo.
- Registrar el cliente o la reserva en `ListaEspera`.
- Guardar:
  - id de reserva
  - id de vuelo solicitado
  - fecha de solicitud
  - prioridad u orden de llegada
  - estado en espera
- Cambiar el estado de la reserva a **En espera**, si así se definió.
- Guardar cambios en MySQL.
- Mostrar mensaje indicando que fue agregado a la lista de espera.

### Flujo para cancelación o liberación de cupo

- Detectar cuando una reserva es cancelada.
- Aumentar los asientos disponibles del vuelo.
- Consultar si existen personas en lista de espera para ese vuelo.
- Ordenar la lista de espera por prioridad o fecha de ingreso.
- Seleccionar al primer candidato en espera.
- Asignar automáticamente el cupo liberado.
- Actualizar su reserva al estado correspondiente:
  - Confirmada
  - Reprogramada
- Descontar nuevamente el asiento del vuelo.
- Registrar el movimiento en historial.
- Notificar en consola que el pasajero fue promovido desde la lista de espera.

## Entregable

- Proyecto actualizado con reprogramación y lista de espera.
- Persistencia en MySQL de historial y estado de espera.
- Evidencia de reasignación de cupos.
- Menú en consola para gestionar cambios y lista de espera.
- Repositorio Git actualizado.
- Repositorio del proyecto con una nueva rama llamada `feature/examen-Nombre`.

## Rúbrica

| Criterio de evaluación         | Nivel 4 - Excelente (90-100)                                              | Nivel 3 - Alto (75-89)                          | Nivel 2 - Básico (60-74)                        | Nivel 1 - Bajo (5-59)                  |
|--------------------------------|----------------------------------------------------------------------------|--------------------------------------------------|--------------------------------------------------|----------------------------------------|
| Reprogramación de reservas     | Cambia reservas correctamente entre vuelos válidos y actualiza disponibilidad. | Funciona con pequeños errores de validación.     | Funciona parcialmente o con inconsistencias.     | No implementa bien la reprogramación. |
| Lista de espera                | Administra adecuadamente pasajeros en espera y reasignación de cupos.     | Funciona con detalles menores.                   | La lógica existe pero es incompleta.             | No se implementa correctamente.        |
| Historial y trazabilidad       | Registra cambios de forma clara y consultable.                            | Registra la mayoría de eventos con pequeños vacíos. | El historial es parcial.                        | No hay trazabilidad suficiente.        |
| Complejidad técnica e integración | La solución resuelve bien la nueva complejidad sin dañar el sistema base. | La integración es buena con detalles menores.   | La integración es parcial o frágil.              | La integración es deficiente.          |