using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;

namespace PackageSorter;

public static class UserCLI
{
        // --- Helpers ---
        private static string Prompt(string message, string? example = null, bool allowEmpty = false)
        {
            while (true)
            {
                Console.Write(message + (example != null ? $" (e.g. {example})" : "") + ": ");
                var input = Console.ReadLine()?.Trim() ?? string.Empty;
                if (!allowEmpty && string.IsNullOrEmpty(input))
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine("Input cannot be empty. Please try again.");
                    Console.ResetColor();
                    continue;
                }
                return input;
            }
        }

        private static int PromptInt(string message, int defaultValue = 0)
        {
            while (true)
            {
                Console.Write(message + $" (default {defaultValue}): ");
                var s = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(s)) return defaultValue;
                if (int.TryParse(s.Trim(), out int v)) return v;
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("Please enter a valid integer.");
                Console.ResetColor();
            }
        }

        private static T PromptEnum<T>(string message, T defaultValue) where T : struct, Enum
        {
            var names = string.Join(", ", Enum.GetNames(typeof(T)));
            while (true)
            {
                Console.Write($"{message} [{names}] (default {defaultValue}): ");
                var s = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(s)) return defaultValue;
                if (Enum.TryParse<T>(s.Trim(), true, out var v)) return v;
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine($"Allowed values: {names}");
                Console.ResetColor();
            }
        }

        private static bool Confirm(string message)
        {
            while (true)
            {
                Console.Write(message + " (Y/N): ");
                var r = Console.ReadLine()?.Trim().ToUpperInvariant();
                if (string.IsNullOrEmpty(r)) continue;
                if (r == "Y" || r == "YES") return true;
                if (r == "N" || r == "NO") return false;
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("Please answer Y or N.");
                Console.ResetColor();
            }
        }

        // --- Public CLI actions ---

        // Overload for callers (tests) that pass only packages + path: treat as skipConfirm=true
        public static void AddPackageFromConsole(List<Package> packages, string packagesPath)
        {
            AddPackageFromConsole(packages, packagesPath, skipConfirm: true);
        }

        public static void AddPackageFromConsole(List<Package> packages, string packagesPath, bool skipConfirm = false)
        {
            int nextId = packages.Any() ? packages.Max(p => p.ID) + 1 : 1;
            Console.WriteLine($"Adding new package (ID {nextId})");

            var address = Prompt("Address", "123 Main St");
            var customer = Prompt("Customer name", "John Doe");
            var weight = PromptInt("Weight (integer)", 1);
            var size = PromptEnum<PackageSize>("Size", PackageSize.Small);
            var cod = PromptInt("COD (integer, 0 if none)", 0);

            var p = new Package
            {
                ID = nextId,
                Address = address,
                CustomerName = customer,
                Weight = weight,
                Size = size,
                COD = cod,
                Status = PackageStatus.Pending,
                DeliveryAttempts = 0
            };

            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine($"Package summary: ID {p.ID}, {p.CustomerName}, {p.Address}, weight {p.Weight}, size {p.Size}, COD {p.COD}");
            Console.ResetColor();

            if (skipConfirm || Confirm("Save this package?"))
            {
                packages.Add(p);
                SavePackagesToJson(packages, packagesPath);
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"Package {p.ID} added and saved.");
                Console.ResetColor();
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("Operation cancelled.");
                Console.ResetColor();
            }
        }

        // Overload for callers (tests) that pass only customers + path: treat as skipConfirm=true
        public static void AddCustomerFromConsole(List<Customer> customers, string customersPath)
        {
            AddCustomerFromConsole(customers, customersPath, skipConfirm: true);
        }

        public static void AddCustomerFromConsole(List<Customer> customers, string customersPath, bool skipConfirm = false)
        {
            Console.WriteLine("Adding new customer");
            var name = Prompt("Name", "Alice Smith");
            var address = Prompt("Address", "123 Main St");

            var c = new Customer { Name = name, Address = address, IsAtHome = false };

            Console.WriteLine($"Customer summary: {c.Name}, {c.Address}");
            if (skipConfirm || Confirm("Save this customer?"))
            {
                customers.Add(c);
                SaveCustomersToJson(customers, customersPath);
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"Customer {c.Name} added and saved.");
                Console.ResetColor();
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("Operation cancelled.");
                Console.ResetColor();
            }
        }

        public static void SavePackagesToJson(List<Package> packages, string filePath)
        {
            try
            {
                var options = new JsonSerializerOptions { WriteIndented = true };
                string json = JsonSerializer.Serialize(packages, options);
                File.WriteAllText(filePath, json);
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"Error saving packages: {ex.Message}");
                Console.ResetColor();
            }
        }

        public static void SaveCustomersToJson(List<Customer> customers, string filePath)
        {
            try
            {
                var options = new JsonSerializerOptions { WriteIndented = true };
                string json = JsonSerializer.Serialize(customers, options);
                File.WriteAllText(filePath, json);
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"Error saving customers: {ex.Message}");
                Console.ResetColor();
            }
        }

        // --- Main interactive loop ---
        public static void execute(LoadData simData)
        {
            if (simData == null) throw new ArgumentNullException(nameof(simData));

            bool running = true;
            while (running)
            {
                Console.WriteLine($"\nCurrent day: {DaySimulator.currDay.Date}");
                Console.WriteLine("Actions:");
                Console.WriteLine("(A) Add package — add a package");
                Console.WriteLine("(C) Add customer — add a customer");
                Console.WriteLine("(L) List — show packages and customers");
                Console.WriteLine("(N) Next day — advance to next day");
                Console.WriteLine("(E) Export & Exit — export data and exit");
                Console.WriteLine("(H) Help — show this menu");
                Console.Write("Choose action: ");
                var input = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(input)) continue;

                switch (input.Trim().ToUpperInvariant())
                {
                    case "A":
                    case "ADD":
                        AddPackageFromConsole(simData.packages, Paths.PackagesJson, skipConfirm: false);
                        break;

                    case "C":
                    case "CUSTOMER":
                    case "ADD-CUSTOMER":
                        AddCustomerFromConsole(simData.customers, Paths.CustomersJson, skipConfirm: false);
                        break;

                    case "L":
                    case "LIST":
                        Console.WriteLine("\nPackages:");
                        foreach (var p in simData.packages)
                            Console.WriteLine($"ID {p.ID}: {p.CustomerName}, {p.Address}, status: {p.Status}");
                        Console.WriteLine("\nCustomers:");
                        foreach (var c in simData.customers)
                            Console.WriteLine($"{c.Name}, {c.Address}, at home: {c.IsAtHome}");
                        break;

                    case "N":
                    case "NEXT":
                    case "ND":
                        DeliveryController.DistributePackages(simData.packages, simData.couriers, simData.customers);
                        DaySimulator.PrepNextDay(simData.packages, simData.customers, simData.couriers);
                        foreach (var c in simData.couriers) c.ResetDay();
                        break;

                    case "E":
                    case "EXIT":
                    case "EXPORT":
                        ExportData.ExportDelivered(simData.packages);
                        ExportData.ExportReturned(simData.packages);
                        ExportData.ExportCourierInfo(simData.couriers);
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine("Export completed. Exiting.");
                        Console.ResetColor();
                        running = false;
                        break;

                    case "H":
                    case "?":
                    case "HELP":
                        // Menu already prints help; add small note
                        Console.WriteLine("Tip: Use the letter in parentheses (e.g. A, C, L, N, E, H) for quick commands.");
                        break;

                    default:
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.WriteLine("Unknown action. Use A, C, L, N, E or H.");
                        Console.ResetColor();
                        break;
                }
            }
        }
    }