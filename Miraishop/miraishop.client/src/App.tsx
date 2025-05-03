import { useEffect, useState } from 'react';
import './App.css';

// 定義 HelloWorld 回應的介面
interface HelloResponse {
    message: string;
    timestamp: string;
}

// 保留原本的 Forecast 介面以供參考
interface Forecast {
    date: string;
    temperatureC: number;
    temperatureF: number;
    summary: string;
}

function App() {
    // 狀態管理
    const [helloMessage, setHelloMessage] = useState<string>('');
    const [loading, setLoading] = useState<boolean>(true);
    const [name, setName] = useState<string>('');
    const [greeting, setGreeting] = useState<string>('');
    const [forecasts, setForecasts] = useState<Forecast[]>();

    // 在元件載入時呼叫 API
    useEffect(() => {
        fetchHelloWorld();
        populateWeatherData();
    }, []);

    // 顯示 Hello World 的內容
    const helloWorldContent = loading
        ? <p><em>正在載入 Hello World 訊息...</em></p>
        : <div className="hello-message">{helloMessage}</div>;

    // 原本的天氣預報內容
    const weatherContent = forecasts === undefined
        ? <p><em>Loading... Please refresh once the ASP.NET backend has started.</em></p>
        : <table className="table table-striped" aria-labelledby="tableLabel">
            <thead>
                <tr>
                    <th>Date</th>
                    <th>Temp. (C)</th>
                    <th>Temp. (F)</th>
                    <th>Summary</th>
                </tr>
            </thead>
            <tbody>
                {forecasts.map(forecast =>
                    <tr key={forecast.date}>
                        <td>{forecast.date}</td>
                        <td>{forecast.temperatureC}</td>
                        <td>{forecast.temperatureF}</td>
                        <td>{forecast.summary}</td>
                    </tr>
                )}
            </tbody>
        </table>;

    // 處理表單提交
    const handleSubmit = async (e: React.FormEvent) => {
        e.preventDefault();
        if (name.trim() === '') return;
        
        try {
            const response = await fetch('https://localhost:44341/helloworld', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json'
                },
                body: JSON.stringify({ name })
            });
            
            if (response.ok) {
                const data = await response.json();
                setGreeting(data.message);
            } else {
                setGreeting('無法取得伺服器回應');
            }
        } catch (error) {
            console.error('發生錯誤:', error);
            setGreeting('發生錯誤，請稍後再試');
        }
    };

    return (
        <div className="container mt-4">
            <div className="card mb-4">
                <div className="card-header bg-primary text-white">
                    <h2>Hello World 示範</h2>
                </div>
                <div className="card-body">
                    <p>這個元件示範了從伺服器取得 Hello World 訊息。</p>
                    {helloWorldContent}
                    
                    <hr className="my-4" />
                    
                    <h3>傳送你的名字給伺服器</h3>
                    <form onSubmit={handleSubmit} className="mb-3">
                        <div className="input-group">
                            <input 
                                type="text" 
                                className="form-control" 
                                placeholder="請輸入你的名字" 
                                value={name}
                                onChange={(e) => setName(e.target.value)}
                            />
                            <button type="submit" className="btn btn-primary">送出</button>
                        </div>
                    </form>
                    
                    {greeting && (
                        <div className="alert alert-success mt-3">
                            {greeting}
                        </div>
                    )}
                </div>
            </div>
            
            <div className="card">
                <div className="card-header bg-secondary text-white">
                    <h2 id="tableLabel">Weather forecast</h2>
                </div>
                <div className="card-body">
                    <p>This component demonstrates fetching data from the server.</p>
                    {weatherContent}
                </div>
            </div>
        </div>
    );

    // 取得 Hello World 訊息
    async function fetchHelloWorld() {
        try {
            setLoading(true);
            const response = await fetch('https://localhost:44341/helloworld');
            if (response.ok) {
                const data: HelloResponse = await response.json();
                setHelloMessage(`${data.message} (伺服器時間: ${new Date(data.timestamp).toLocaleString()})`); 
            } else {
                setHelloMessage('無法取得 Hello World 訊息');
            }
        } catch (error) {
            console.error('發生錯誤:', error);
            setHelloMessage('發生錯誤，請稍後再試');
        } finally {
            setLoading(false);
        }
    }

    // 原本的天氣預報資料獲取函數
    async function populateWeatherData() {
        const response = await fetch('https://localhost:44341/weatherforecast');
        if (response.ok) {
            const data = await response.json();
            setForecasts(data);
        }
    }
}

export default App;