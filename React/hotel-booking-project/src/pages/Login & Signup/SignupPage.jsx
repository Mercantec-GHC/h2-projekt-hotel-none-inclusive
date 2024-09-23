import './SignupPage.css'
import InputField from "../../components/Signup & Login/InputField.jsx";
import FormTitle from "../../components/Signup & Login/FormTitle.jsx";
import FormButton from "../../components/Signup & Login/FormButton.jsx";
import { useState } from "react";
import { Link } from "react-router-dom";
import axios from 'axios';

function SignupPage() {
    const [email, setEmail] = useState('');
    const [password, setPassword] = useState('');

    const handleSubmit = async (e) => {
        e.preventDefault()
        console.log('Form Submitted');
        try {
            console.log({email, password});
            await axios.post("https://localhost:7207/api/Auth/register",
                {email, password}
            );
            console.log('Register successful');

        } catch (error) {
            console.error('Authentication error:', error.response.data);
        }
    }

    return (
        <div className="signup-container">
            <FormTitle title="Sign Up" />
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

                <FormButton type="submit" text="Sign Up" />
            </form>

            <p>Already have an account? <Link to="/login">Log In</Link></p>
        </div>
    )
}

export default SignupPage
