import './LoginPage.css';
import InputField from "../../components/Signup & Login/InputField.jsx";
import FormTitle from "../../components/Signup & Login/FormTitle.jsx";
import FormButton from "../../components/Signup & Login/FormButton.jsx";
import { useState } from 'react';
import { useNavigate } from 'react-router-dom';
import axios from 'axios';
import { useAuth } from '../../context/AuthContext';

function LoginPage() {
    const [email, setEmail] = useState('');
    const [password, setPassword] = useState('');
    const [error, setError] = useState('');
    const navigate = useNavigate();
    const { login } = useAuth();

    const handleSubmit = async (e) => {
        e.preventDefault();
        console.log({ email, password });
        try {
            const response = await axios.post("https://localhost:7207/api/Auth/login", { email, password });
            if (response.status === 200) {
                localStorage.setItem('token', response.data);
                login();
                navigate('/');
            } else {
                setError('Login failed. Please check your credentials.');
            }
            // eslint-disable-next-line no-unused-vars
        } catch (error) {
            setError('Login failed. Please check your credentials.');
        }
    };

    return (
        <div className="login-container">
            <FormTitle title="Log In" />
            <form onSubmit={handleSubmit}>
                <InputField
                    labelText="Email"
                    inputType="email"
                    inputId="email"
                    inputName="email"
                    value={email}
                    onChange={(e) => setEmail(e.target.value)}
                />
                <InputField
                    labelText="Password"
                    inputType="password"
                    inputId="password"
                    inputName="password"
                    value={password}
                    onChange={(e) => setPassword(e.target.value)}
                />
                <FormButton type="submit" text="Log In" />
            </form>
            {error && <p className="error-message">{error}</p>}
        </div>
    );
}

export default LoginPage;
