# TravelAPI

## Descripción

Este repositorio contiene el código fuente de un conjunto de APIs que proporcionan funcionalidades relacionadas con la gestión de hoteles, habitaciones y reservas, así como la manipulación de datos de huéspedes y contactos de emergencia.

## Tecnologías Utilizadas

- C# .NET 7
- Mongo DB
- SendGrid
- JWT

## Enlace del proyecto desplegado

- [API de Hoteles](http://www.cgtravelapi.somee.com/swagger/index.html)

## APIs Implementadas

1. **Crear Hotel:** Permite la creación de nuevos hoteles con información detallada.

2. **Asignar Habitaciones:** Permite asignar habitaciones a un hotel específico.

3. **Editar Hotel:** Permite la modificación de los detalles de un hotel existente.

4. **Editar Habitación:** Permite la modificación de los detalles de una habitación existente.

5. **Habilitar/Deshabilitar Hotel:** Permite habilitar o deshabilitar la disponibilidad de un hotel.

6. **Habilitar/Deshabilitar Habitación:** Permite habilitar o deshabilitar la disponibilidad de una habitación.

7. **Listar Reservas:** Proporciona una lista de todas las reservas realizadas.

8. **Buscar Hoteles Disponibles:** Permite buscar hoteles con habitaciones disponibles para reserva utilizando varios filtros, como fecha de entrada, fecha de salida, cantidad de personas y ciudad de destino.

9. **Realizar Reserva:** Permite realizar una reserva de habitación para un huésped con la información detallada requerida.

## Instrucciones de Uso

1. Clonar el repositorio a nivel local.
2. Configurar la conexión a la base de datos Mongo DB.
3. Compilar y ejecutar la solución en un entorno de desarrollo para .NET.
4. Utilizar la UI de Swagger o si lo prefiere puede utilizar herramientas como Postman para probar las distintas APIs expuestas.

## Colecciones de base de datos

### Hotels

```Mongo DB
{
  "_id": { "$oid": "65e93bae308aa74ccc9c9541" },
  "Name": "Hotel 1",
  "Address": {
    "City": "Medellin",
    "Country": "Colombia"
  },
  "Enabled": true,
  "Rooms": [
    {
      "_id": {
        "$oid": "65e922989468f920e0a5025e"
      },
      "Number": "101",
      "Type": "Sencilla",
      "BaseRate": { "$numberDouble": "200.0" },
      "Taxes": { "$numberDouble": "10.0" },
      "Location": "Primer Piso",
      "Capacity": { "$numberInt": "2" },
      "Enabled": true
    }
  ]
}
```

### Reservations

```Mongo DB
{
  "_id": { "$oid": "65e93c0e308aa74ccc9c9542" },
  "CheckInDate": {
    "$date": { "$numberLong": "1709681625943" }
  },
  "CheckOutDate": {
    "$date": { "$numberLong": "1709681625943" }
  },
  "NumberOfGuests": { "$numberInt": "1" },
  "HotelId": {
    "$oid": "65e79cab206fc10efad9d761"
  },
  "RoomId": {
    "$oid": "65e7a5911233d7786213c15e"
  },
  "Guests": [
    {
      "FirstName": "Claudia",
      "LastName": "Guarin",
      "BirthDate": {
        "$date": {
          "$numberLong": "1709681625943"
        }
      },
      "Gender": "Femenino",
      "DocumentType": "CC",
      "DocumentNumber": "2938473",
      "Email": "claug@gmail.com",
      "ContactPhone": "3003651176"
    }
  ],
  "EmergencyContact": {
    "FullName": "Maria Ospina Ospina",
    "ContactPhone": "6043664587"
  }
}
```
