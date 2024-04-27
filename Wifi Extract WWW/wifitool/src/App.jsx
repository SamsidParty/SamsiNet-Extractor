import { Fragment, useState } from 'react'
import './App.css'
import { Input, Divider, Code, Button, Modal, ModalBody, ModalContent, ModalFooter, ModalOverlay, ModalCloseButton, ModalHeader } from '@chakra-ui/react';
import {QRCodeSVG} from 'qrcode.react';

window.SavedPasswords = [];

//Called By C#, Sends Passwords To JS
window.AddEntries = (passwords) => {
    window.SavedPasswords = passwords;

    setRerender(rerender + 1);
}

function App() {
    var [rerender, setRerender] = useState(0);
    window.rerender = rerender;
    window.setRerender = setRerender;

    var [search, setSearch] = useState("");
    var [isQROpen, setIsQROpen] = useState(false);
    var [currentPassword, setCurrentPassword] = useState({ Key: "", Value: "" }); // The Password Displayed By The QR Code

    var openQRCode = (password) => {
        setCurrentPassword(password);
        setIsQROpen(true);
    }

    return (
        <div className={'app' + ((rerender > 0) ? " loaded" : "")}>
            <div className='header'>
                <h2>Wi-Fi Passwords</h2>
                <Input value={search} onChange={(e) => setSearch(e.target.value)} variant='filled' placeholder={'\ueb1c  Search'} />
            </div>

            <div className='wifiList'>
                {
                    SavedPasswords.map((l_password, l_index) => {

                        if (search.length == 0 || l_password.Key.toLowerCase().includes(search.toLowerCase()) || l_password.Value.toLowerCase().includes(search.toLowerCase())) {
                            return (<Wifi key={l_index} index={l_index} password={l_password} openQRCode={openQRCode}></Wifi>)
                        }
                        return (<Fragment key={l_index}></Fragment>);

                    })
                }
            </div>
            <Modal blockScrollOnMount={false} isCentered isOpen={isQROpen} onClose={() => setIsQROpen(false)}>
                <ModalOverlay />
                <ModalContent>
                    <ModalHeader>QR Code</ModalHeader>
                    <ModalCloseButton />
                    <ModalBody>
                        <QRCodeSVG size="256" className='QRCode' value={`WIFI:S:${currentPassword.Key};T:WPA;P:${currentPassword.Value};;`} />
                    </ModalBody>

                    <ModalFooter>

                    </ModalFooter>
                </ModalContent>
            </Modal>
        </div>
    )
}

function Wifi(props) {

    var password = props.password;
    var index = props.index

    var copyPassword = (password) => {
        if (navigator.clipboard) {
            navigator.clipboard.writeText(password);
        }
    }

    var sharePassword = async (data) => {
        if (navigator.share) {
            CallCSharp("SamsidParty_WiFi_Extract.Frontend, WifiTool", "SharePassword"); // Tells C# To Expand The Window To Make The Share UI Fit
            await navigator.share({
                title: "Password For Network " + data.Key,
                text: "SSID: " + data.Key + "\nPassword: " + data.Value
            });
            CallCSharp("SamsidParty_WiFi_Extract.Frontend, WifiTool", "FinishSharePassword");// Tells C# To Return The Window To Normal Size
        }
    }

    return (
        <>
            <Divider></Divider>
            <div className='wifi'>
                <h2>&#xeb52;</h2>
                <div className='wifiInfo'>
                    <h3>{password.Key}</h3>
                    <div className='password'>
                        <Code>{password.Value}</Code>
                        <Button onClick={() => copyPassword(password.Value)}>&#xea7a;</Button>
                    </div>
                </div>
                <Button onClick={() => sharePassword(password)}>&#xf7bd;</Button>
                <Button onClick={() => props.openQRCode(password)}>&#xeb11;</Button>
            </div>
        </>
    )
}

export default App
