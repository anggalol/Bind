/*
 * Aplikasi pengecek kode soal untuk web application buatan almazary
 * Link: github.com/almazary/new_elearning
 * 
 * Aplikasi ini dibuat untuk .NET Framework 3.5 ke atas dan berbasis Console Application
 * Versi v1.1
 * 
 * Author       : anggalol
 * Copyright    : Copyright (c) 2020
 * License      : CC BY-NC-SA 4.0
 * 
 * Gunakan aplikasi ini dengan bijak.
 */

using System;
using System.IO;

namespace bind
{
    /// <summary>
    /// Kelas utama aplikasi Bind
    /// </summary>
    public class BindApp
    {
        /// <summary>
        /// Kondisi yang harus dipenuhi untuk mendapatkan kode soal
        /// </summary>
        private const string ID_TAG_REQ = "<li id=\"no-pertanyaan-";

        public static void Main()
        {
            // Header aplikasi
            Console.WriteLine("BIND - Question Codes Tracker");
            Console.WriteLine("[Version 1.1, .NET Framework 3.5]");

            // Masuk endless loop untuk memasukkan file HTML secara terus menerus
            while (true)
            {
                // User memasukkan file HTML/HTM di sini
                Console.Write("\nResource File (HTML or HTM): ");
                string htmlFile = Console.ReadLine();

                // Coba untuk membaca file HTML yang dimasukkan oleh user.
                // Jika file tidak ada, tangkap eksepsi FileNotFoundException
                try
                {
                    // Baca semua baris file HTML dan dapatkan banyak pertanyaan
                    string[] htmlLines = File.ReadAllLines(htmlFile);
                    int count = GetManyQuestion(htmlLines);

                    // Dapatkan semua baris pertanyaan dan dapatkan semua kodenya
                    string[] questionLines = GetQuestionLines(htmlLines);
                    int[] codes = GetAllQuestionCodes(questionLines);
                    int[] sortedCodes = GetAllQuestionCodes(questionLines);
                    
                    // Mengurutkan kode agar mudah dibaca
                    QuickSort(sortedCodes, 0, count - 1);

                    // Dapatkan nomor soal pada setiap kode soal
                    int[] nums = new int[count];
                    for (int i = 0; i < count; i++)
                    {
                        for (int j = 0; j < count; j++)
                        {
                            if (sortedCodes[i] == codes[j])
                            {
                                nums[i] = j + 1;
                                break;
                            }
                        }

                        // Print kode soal dan nomor soal yang berkaitan
                        Console.WriteLine("code: {0}\tnum: {1}", sortedCodes[i], nums[i]);
                    }

                    // Cek banyak soal yang terdapat pada file HTML
                    if (count == 0)
                        Console.WriteLine("HTML file not valid");
                    else
                    {
                        Console.WriteLine("--------------------------");
                        Console.WriteLine("Total: {0} ({1} - {2})", count, sortedCodes[0], sortedCodes[count - 1]);
                    }

                    // User memasukkan nomor awal untuk dibagi jawabannya
                    Console.Write("\nStart Num : ");
                    int start = int.Parse(Console.ReadLine());

                    // User memasukkan nomor akhir untuk dibagi jawabannya
                    Console.Write("End Num   : ");
                    int end = int.Parse(Console.ReadLine());

                    Console.WriteLine("--------------------------");

                    // Cek jika angka yang dimasukkan diluar jangkauan array atau jika nomor awal lebih dari nomor akhir
                    if (start < 0 || end > sortedCodes.Length || start > end)
                    {
                        Console.WriteLine("start num and/or end num are out of ranges");

                        // Akhiri loop utama dan lanjutkan loop utama
                        continue;
                    }

                    // User memasukkan jawaban pada setiap kode soal disini
                    // - Update v1.1: Support kode soal yang loncat-loncat
                    for (int i = start; i <= end; i++)
                    {
                        Console.Write("ans[{0}]: ", sortedCodes[i - 1]);
                        Console.ReadLine();
                    }

                    // Akhir loop, ulangi dari awal lagi
                    Console.WriteLine("--------------------------");
                }
                catch (FileNotFoundException)
                {
                    // Kasus ketika file tidak ditemukan
                    Console.WriteLine("Cannot find the '{0}' file.", htmlFile);
                }
            }
        }

        /// <summary>
        /// Dapatkan semua kode soal pada setiap soal
        /// </summary>
        /// <param name="questionLines">Kumpulan baris pertanyaan</param>
        /// <returns>array dari kode</returns>
        private static int[] GetAllQuestionCodes(string[] questionLines)
        {
            int[] codes = new int[questionLines.Length];
            for (int i = 0; i < questionLines.Length; i++)
                codes[i] = int.Parse(questionLines[i].Trim().Substring(22, 3));

            return codes;
        }

        /// <summary>
        /// Dapatkan banyak pertanyaan pada file HTML
        /// </summary>
        /// <param name="lines">Baris file HTML</param>
        /// <returns>Banyak pertanyaan</returns>
        private static int GetManyQuestion(string[] lines)
        {
            int count = 0;
            for (int i = 0; i < lines.Length; i++)
            {
                if (lines[i].Contains(ID_TAG_REQ))
                    count += 1;
            }

            return count;
        }

        /// <summary>
        /// Dapatkan kumpulan baris yang merupakan baris pertanyaan
        /// </summary>
        /// <param name="lines">Baris file HTML</param>
        /// <returns>array pertanyaan</returns>
        private static string[] GetQuestionLines(string[] lines)
        {
            int count = GetManyQuestion(lines);
            string[] questionLines = new string[count];
            int current = 0;
            for (int i = 0; i < lines.Length; i++)
            {
                if (lines[i].Contains(ID_TAG_REQ))
                {
                    questionLines[current] = lines[i];
                    current++;
                }
            }

            return questionLines;
        }

        /// <summary>
        /// Pengurut anggota array dengan algoritma QuickSort
        /// </summary>
        /// <param name="unsortedArr">Array yang akan diurutkan elemen-elemennya</param>
        /// <param name="start">Elemen pertama yang akan diurutkan</param>
        /// <param name="end">Elemen terakhir yang akan diurutkan</param>
        private static void QuickSort(int[] unsortedArr, int start, int end)
        {
            int i;
            if (start < end)
            {
                i = Partition(unsortedArr, start, end);

                QuickSort(unsortedArr, start, i - 1);
                QuickSort(unsortedArr, i + 1, end);
            }
        }

        /// <summary>
        /// Partisikan array menjadi dua subarray
        /// </summary>
        /// <param name="unsortedArr">Array yang akan dipartisikan</param>
        /// <param name="start">Elemen pertama yang akan dipartisikan</param>
        /// <param name="end">Elemen terakhir yang akan dipartisikan</param>
        /// <returns>pivot</returns>
        /// <remarks>
        /// Partisi array merupakan bagian dari algoritma pengurutan QuickSort
        /// </remarks>
        private static int Partition(int[] unsortedArr, int start, int end)
        {
            int temp;
            int p = unsortedArr[end];
            int i = start - 1;

            for (int j = start; j <= end - 1; j++)
            {
                if (unsortedArr[j] <= p)
                {
                    i++;
                    temp = unsortedArr[i];
                    unsortedArr[i] = unsortedArr[j];
                    unsortedArr[j] = temp;
                }
            }

            temp = unsortedArr[i + 1];
            unsortedArr[i + 1] = unsortedArr[end];
            unsortedArr[end] = temp;

            return i + 1;
        }
    }
}
