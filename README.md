# WeatherApp

Weather App with C# Backend and SQL Integration
This is a simple weather application built with C# for the backend and SQL for database management. The app allows users to search for a city and get real-time weather information, including key details such as temperature, humidity, wind speed, and more, at the exact moment the search is made.

Key Features:
City Search: Users can input the name of any city to retrieve current weather data.

Real-Time Weather: The app fetches live weather data and displays key metrics for the selected city.

Search History: The app stores the history of previous city searches in an SQL database, allowing users to easily access weather data for cities they've searched for before.

Technologies Used:
Backend: C#

Database: SQL (for storing search history)

API Integration: Weather data is retrieved from a weather API (e.g., OpenWeatherMap or another similar service).

How It Works:
The user enters a city name into the search field.

The app queries an external weather API for current weather data for that city.

The retrieved weather information is displayed to the user.

The search query is logged and stored in an SQL database, allowing users to view their search history.

Image 1: UI when opening the app

![image](https://github.com/user-attachments/assets/11460be2-54c0-4547-8c1a-33f31c478d5d)

Image 2: After user has searched some cities and we see history

![image](https://github.com/user-attachments/assets/f1e03c3d-e056-475f-a4d7-5943c96926c0)
