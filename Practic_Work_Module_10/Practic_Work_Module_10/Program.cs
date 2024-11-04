using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Practic_Work_Module_10
{
    using System;

    namespace HotelManagementSystem
    {
        using System;

        namespace HotelManagementSystem
        {
            // Подсистема бронирования номеров
            public class RoomBookingSystem
            {
                public bool CheckAvailability(int roomId)
                {
                    Console.WriteLine($"Checking availability for room {roomId}...");
                    return true; // Проверка доступности
                }

                public void BookRoom(int roomId)
                {
                    Console.WriteLine($"Room {roomId} has been successfully booked.");
                }

                public void CancelBooking(int roomId)
                {
                    Console.WriteLine($"Booking for room {roomId} has been canceled.");
                }
            }

            // Подсистема ресторана
            public class RestaurantSystem
            {
                public void ReserveTable(int tableId)
                {
                    Console.WriteLine($"Table {tableId} has been reserved.");
                }

                public void OrderFood(string foodItem)
                {
                    Console.WriteLine($"Food item '{foodItem}' has been ordered.");
                }

                public void CancelTableReservation(int tableId)
                {
                    Console.WriteLine($"Reservation for table {tableId} has been canceled.");
                }
            }

            // Подсистема управления мероприятиями
            public class EventManagementSystem
            {
                public void ReserveConferenceHall(string hallName)
                {
                    Console.WriteLine($"Conference hall '{hallName}' has been reserved.");
                }

                public void OrderEquipment(string equipment)
                {
                    Console.WriteLine($"Equipment '{equipment}' has been ordered for the event.");
                }

                public void CancelEvent(string hallName)
                {
                    Console.WriteLine($"Reservation for conference hall '{hallName}' has been canceled.");
                }
            }

            // Подсистема уборки
            public class CleaningService
            {
                public void ScheduleCleaning(int roomId, DateTime date)
                {
                    Console.WriteLine($"Cleaning for room {roomId} scheduled on {date.ToShortDateString()}.");
                }

                public void PerformCleaning(int roomId)
                {
                    Console.WriteLine($"Cleaning performed for room {roomId}.");
                }

                public void RequestImmediateCleaning(int roomId)
                {
                    Console.WriteLine($"Immediate cleaning requested for room {roomId}.");
                }
            }

            // Класс фасада
            public class HotelFacade
            {
                private readonly RoomBookingSystem _roomBookingSystem = new RoomBookingSystem();
                private readonly RestaurantSystem _restaurantSystem = new RestaurantSystem();
                private readonly EventManagementSystem _eventManagementSystem = new EventManagementSystem();
                private readonly CleaningService _cleaningService = new CleaningService();

                // Бронирование номера с заказом еды и услуг уборки
                public void BookRoomWithService(int roomId, string foodItem, DateTime cleaningDate)
                {
                    Console.WriteLine("\n--- Booking Room with Services ---");
                    if (_roomBookingSystem.CheckAvailability(roomId))
                    {
                        _roomBookingSystem.BookRoom(roomId);
                        _restaurantSystem.OrderFood(foodItem);
                        _cleaningService.ScheduleCleaning(roomId, cleaningDate);
                        Console.WriteLine("Room booking with additional services completed.\n");
                    }
                    else
                    {
                        Console.WriteLine("Room not available.");
                    }
                }

                // Организация мероприятия с бронированием номеров и заказом оборудования
                public void OrganizeEvent(string hallName, string equipment, int roomId)
                {
                    Console.WriteLine("\n--- Organizing Event ---");
                    _eventManagementSystem.ReserveConferenceHall(hallName);
                    _eventManagementSystem.OrderEquipment(equipment);
                    _roomBookingSystem.BookRoom(roomId);
                    Console.WriteLine("Event organization with room booking completed.\n");
                }

                // Бронирование стола с заказом такси
                public void ReserveTableWithTaxi(int tableId)
                {
                    Console.WriteLine("\n--- Reserving Table with Taxi ---");
                    _restaurantSystem.ReserveTable(tableId);
                    Console.WriteLine("Taxi ordered for the restaurant reservation.\n");
                }

                // Отмена бронирования номера
                public void CancelRoomBooking(int roomId)
                {
                    Console.WriteLine("\n--- Canceling Room Booking ---");
                    _roomBookingSystem.CancelBooking(roomId);
                }

                // Отмена бронирования стола
                public void CancelTableReservation(int tableId)
                {
                    Console.WriteLine("\n--- Canceling Table Reservation ---");
                    _restaurantSystem.CancelTableReservation(tableId);
                }

                // Отмена мероприятия
                public void CancelEventReservation(string hallName)
                {
                    Console.WriteLine("\n--- Canceling Event Reservation ---");
                    _eventManagementSystem.CancelEvent(hallName);
                }

                // Организация уборки по запросу
                public void RequestImmediateCleaning(int roomId)
                {
                    Console.WriteLine("\n--- Requesting Immediate Cleaning ---");
                    _cleaningService.RequestImmediateCleaning(roomId);
                }
            }

            // Точка входа
            public class Program
            {
                public static void Main()
                {
                    HotelFacade hotelFacade = new HotelFacade();

                    // Сценарий: Бронирование номера с услугами ресторана и уборки
                    hotelFacade.BookRoomWithService(roomId: 101, foodItem: "Pizza", cleaningDate: DateTime.Now.AddDays(1));

                    // Сценарий: Организация мероприятия с бронированием оборудования и номеров
                    hotelFacade.OrganizeEvent(hallName: "Grand Ballroom", equipment: "Projector", roomId: 202);

                    // Сценарий: Бронирование стола в ресторане с вызовом такси
                    hotelFacade.ReserveTableWithTaxi(tableId: 12);

                    // Отмена бронирования номера
                    hotelFacade.CancelRoomBooking(roomId: 101);

                    // Отмена бронирования стола
                    hotelFacade.CancelTableReservation(tableId: 12);

                    // Отмена мероприятия
                    hotelFacade.CancelEventReservation(hallName: "Grand Ballroom");

                    // Организация уборки по запросу
                    hotelFacade.RequestImmediateCleaning(roomId: 202);
                }
            }
        }

    }

}
