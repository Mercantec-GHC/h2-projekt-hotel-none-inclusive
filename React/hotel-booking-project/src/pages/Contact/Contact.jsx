import './Contact.css';
import { useState } from 'react';
import FormTitle from "../../components/Signup & Login/FormTitle.jsx";
import FormButton from "../../components/Signup & Login/FormButton.jsx";
import InputField from "../../components/Signup & Login/InputField.jsx";

const Contact = () => {
    // Set the default values
    const [emailToId] = useState('noreply@leeloo.dk');
    const [emailToName] = useState('Support');
    const [emailSubject, setEmailSubject] = useState('');
    const [emailBody, setEmailBody] = useState('');
    const [response, setResponse] = useState('');

    const handleSubmit = async (e) => {
        e.preventDefault();

        const emailData = {
            emailToId,
            emailToName,
            emailSubject,
            emailBody
        };

        try {
            const res = await fetch('https://localhost:7207/Mail', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json',
                    'accept': 'text/plain'
                },
                body: JSON.stringify(emailData)
            });

            if (res.ok) {
                // const result = await res.json();
                setResponse('Email sent successfully!');
                // Clear the subject and body fields after sending
                setEmailSubject('');
                setEmailBody('');
            } else {
                setResponse('Failed to send email.');
            }
        } catch (error) {
            console.error('Error:', error);
            setResponse('An error occurred while sending the email.');
        }
    };

    return (
        <div className="contact-container">
            <FormTitle title="Contact" />
            <form onSubmit={handleSubmit}>
                {/* Removed Email To (ID) and Email To (Name) fields */}
                <InputField
                    labelText="Subject"
                    inputType="subject"
                    inputId="subject"
                    inputName="subject"
                    value={emailSubject}
                    onChange={(e) => setEmailSubject(e.target.value)}
                />
                <InputField
                    labelText="Message"
                    inputType="message"
                    inputId="message"
                    inputName="message"
                    value={emailBody}
                    onChange={(e) => setEmailBody(e.target.value)}
                />
                <FormButton type="submit" text="Send e-mail" />
            </form>
            {response && <p>{response}</p>}
        </div>
    );
};

export default Contact;
