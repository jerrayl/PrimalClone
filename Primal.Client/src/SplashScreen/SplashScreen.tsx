import { GoogleLogin, GoogleOAuthProvider } from "@react-oauth/google";
import primallogo from "../assets/primallogo.png";
import Cookies from 'js-cookie';
import { useNavigate } from "react-router-dom";
import { AUTHORIZATION, ENCOUNTER_ID } from "../Game/utils/constants";

export const SplashScreen = () => {
  const navigate = useNavigate();

  if (Cookies.get(ENCOUNTER_ID)) {
    navigate('/game');
  } else if (Cookies.get(AUTHORIZATION)) {
    navigate('/main-menu');
  }

  return (
    <GoogleOAuthProvider clientId={import.meta.env.VITE_GOOGLE_CLIENT_ID}>
      <div className="flex flex-col caret-transparent h-screen bg-bg1">
        <div className="flex justify-center">
          <img src={primallogo} alt="primal logo" />
        </div>
        <div className="flex justify-center">
          <GoogleLogin
            onSuccess={credentialResponse => {
              if (credentialResponse.credential) {
                Cookies.set(AUTHORIZATION, credentialResponse.credential, { secure: true });
                navigate('/main-menu');
              }
            }}
            onError={() => {
              console.log('Login Failed');
            }}
            useOneTap
          />
        </div>
      </div>
    </GoogleOAuthProvider>
  );
};