import 'package:flutter/material.dart';
import 'package:http/http.dart' as http;
import 'dart:convert';
import 'package:flutter_module_1/detail_screen.dart'; 
// ⚠️ หมายเหตุ: ตรงคำว่า flutter_module_1 ให้เปลี่ยนเป็น "ชื่อโปรเจกต์" ของคุณตามที่ระบุใน pubspec.yaml ครับ
void main() {
  runApp(const MyApp());
}

class MyApp extends StatelessWidget {
  const MyApp({super.key});

  @override
  Widget build(BuildContext context) {
    return MaterialApp(home: const JokeListScreen());
  }
}

class JokeListScreen extends StatefulWidget {
  const JokeListScreen({super.key});

  @override
  State<JokeListScreen> createState() => _JokeListScreenState();
}

class _JokeListScreenState extends State<JokeListScreen> {
  // 1. เปลี่ยนจากตัวแปรเดี่ยว ให้เป็น List เพื่อเก็บข้อมูลหลายๆ ตัว
  List _jokesList = [];
  bool _isLoading = false;
  String _errorMessage = "";

  Future<void> fetchJokes() async {
    setState(() {
      _isLoading = true;
      _errorMessage = "";
    });

    // เปลี่ยน URL เป็น IP สำหรับ Emulator ของคุณ
    final url = Uri.parse(
      'http://10.0.2.2:5000/api/Products',
    ); //ip นี้สำหรับรันใน locol host

    try {
      final response = await http.get(url);

      if (response.statusCode == 200) {
        // แปลงข้อมูล JSON ที่ได้มา (ซึ่งเป็น List ของ Object)
        final List data = jsonDecode(response.body);

        // 2. อัปเดต State โดยเอาข้อมูลทั้ง List ไปเก็บไว้ใน _jokesList
        setState(() {
          _jokesList = data;
          _isLoading = false;
        });
      } else {
        setState(() {
          _errorMessage = "เกิดข้อผิดพลาด: ${response.statusCode}";
          _isLoading = false;
        });
      }
    } catch (e) {
      setState(() {
        _errorMessage = "ไม่สามารถเชื่อมต่อเครื่องคอมพิวเตอร์ (Localhost) ได้";
        _isLoading = false;
      });
    }
  }

  @override
  void initState() {
    super.initState();
    fetchJokes(); // ให้ดึงข้อมูลทันทีที่เปิดหน้านี้ขึ้นมา
  }

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: AppBar(
        title: const Text('Data List จาก Localhost'),
        backgroundColor: Colors.blueAccent,
      ),
      // 3. ส่วนแสดงผลบนหน้าจอ (UI)
      body: _isLoading
          ? const Center(child: CircularProgressIndicator()) // แสดงตอนกำลังโหลด
          : _errorMessage.isNotEmpty
          ? Center(
              child: Text(
                _errorMessage,
                style: const TextStyle(color: Colors.red),
              ),
            ) // แสดงตอน Error
          : _jokesList.isEmpty
          ? const Center(
              child: Text('ไม่มีข้อมูลในระบบ'),
            ) // แสดงตอน List ว่างเปล่า
          : ListView.builder(
              // วนลูปแสดงผล List ข้อมูลทั้งหมด
              itemCount: _jokesList.length,
              itemBuilder: (context, index) {
                // ดึงข้อมูลแถวปัจจุบัน (index) ออกมาเก็บไว้ในตัวแปร item
                final item = _jokesList[index];

                return Card(
                  margin: const EdgeInsets.symmetric(
                    horizontal: 12,
                    vertical: 6,
                  ),
                  elevation: 2,
                  child: ListTile(
                    // ✨ เพิ่ม onTap ตรงนี้เมื่อกดที่รายการ
                    onTap: () {
                      Navigator.push(
                        context,
                        MaterialPageRoute(
                          // ส่งข้อมูล item (ที่เป็น Map) ไปยังหน้าถัดไป
                          builder: (context) => DetailScreen(item: item),
                        ),
                      );
                    },

                    leading: CircleAvatar(
                      backgroundColor: Colors.blueAccent,
                      child: Text(
                        '${index + 1}',
                        style: const TextStyle(color: Colors.white),
                      ),
                    ),
                    title: Text(
                      item['name'] ?? 'ไม่มีข้อความ',
                      style: const TextStyle(
                        fontWeight: FontWeight.bold,
                        fontSize: 16,
                      ),
                    ),
                    subtitle: Padding(
                      padding: const EdgeInsets.only(top: 6.0),
                      child: Text(
                        item['description'] ?? '',
                        style: TextStyle(
                          color: Colors.grey[700],
                          fontStyle: FontStyle.italic,
                        ),
                      ),
                    ),
                  ),
                );
              },
            ),
      floatingActionButton: FloatingActionButton(
        onPressed: fetchJokes, // กดเพื่อ Refresh ข้อมูลใหม่
        child: const Icon(Icons.refresh),
      ),
    );
  }
}

