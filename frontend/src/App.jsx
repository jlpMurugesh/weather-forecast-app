import React, { useState } from 'react';
import axios from 'axios';
import './App.css';

function App() {
  const [city, setCity] = useState('');
  const [weather, setWeather] = useState(null);
  const [error, setError] = useState(null);

  const fetchWeather = async (e) => {
    if (e.key === 'Enter') {
      e.preventDefault();
      setWeather(null);
      setError(null);

      try {
        const response = await axios.get(`https://localhost:7137/api/weather`, {
          params: { city },
        });

        setWeather(response.data);
      } catch (err) {
        if (err.response) {
          setError('City not found or server error');
        } else if (err.request) {
          setError('No response from server');
        } else {
          setError('Error: ' + err.message);
        }
      }
    }
  };

  return (
    <div className="app">
      <main>
        <div className="search-box">
          <input
            type="text"
            className="search-bar"
            placeholder="Enter city..."
            onChange={(e) => setCity(e.target.value)}
            value={city}
            onKeyDown={fetchWeather}
          />
        </div>

        {error && <div className="error">{error}</div>}

        {weather && (
          <div className="weather-container">
            <div className="weather-box">
              <div className="location-box">
                <div className="location">{weather.city}</div>
              </div>
              <div className="weather-info-box">
                <div className="temp">{Math.round(weather.temperature)}°c</div>
                <div className="weather">{weather.condition}</div>
                <img src={weather.icon} alt={weather.description} />
              </div>
            </div>
          </div>
        )}
      </main>
    </div>
  );
}

export default App;
