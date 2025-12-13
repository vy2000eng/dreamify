namespace dreamify.Domain.Utils;



public static class EmailTemplate
{

    public static string GetConfirmationEmailHtmlPart(string username, string callbackUrl)
    {
       return  $@"
        <div style='font-family: Arial, sans-serif; max-width: 600px; margin: 0 auto;'>
            <h2 style='color: #333; text-align: center;'>Email Verification</h2>
            <p>Hi {username},</p>
            <p>Please verify your email address by clicking the button below:</p>
            <div style='text-align: center; margin: 30px 0;'>
                <a href='{callbackUrl}' style='background-color: #007bff; color: white; padding: 12px 25px; text-decoration: none; border-radius: 4px; display: inline-block;'>Verify Email</a>
            </div>

            <h2 style='color: #333; margin-top: 30px;'>Support Our Mission</h2>
            <p>If you'd like to support this project, you can contribute through:</p>
            <div style='text-align: center; margin: 20px 0;'>
                <a href='https://patreon.com/VladyslavYatsuta?utm_medium=unknown&utm_source=join_link&utm_campaign=creatorshare_creator&utm_content=copyLink' 
                   style='display: inline-block; margin: 10px; padding: 10px 20px; background-color: #FF424D; color: white; text-decoration: none; border-radius: 4px;'>
                    Support on Patreon
                </a>
                <a href='https://www.buymeacoffee.com/vladyslavyatsuta' 
                   style='display: inline-block; margin: 10px; padding: 10px 20px; background-color: #FFDD00; color: black; text-decoration: none; border-radius: 4px;'>
                    Buy me a coffee ☕
                </a>
                <a href='https://www.venmo.com/u/textynews_us' 
                   style='display: inline-block; margin: 10px; padding: 10px 20px; background-color: #008CFF; color: white; text-decoration: none; border-radius: 4px;'>
                    Support via Venmo
                </a>
            </div>

            <h2 style='color: #333; margin-top: 30px;'>Contact Us</h2>
            <p>We value your feedback and are constantly working to improve our service. Reach out to us at:</p>
            <p>Email: <a href='mailto:support@contiguity.xyz' style='color: #007bff;'>support@contiguity.xyz</a></p>
            <p>Website: <a href='https://contiguity.xyz' style='color: #007bff;'>https://contiguity.xyz</a></p>

            <p style='margin-top: 30px;'>Best regards,<br>The Contiguity Team</p>
        </div>";
    }
        public static string GetPasswordResetEmailHtmlPart(string username, string callbackUrl)
    {
       return  $@"
        <div style='font-family: Arial, sans-serif; max-width: 600px; margin: 0 auto;'>
            <h2 style='color: #333; text-align: center;'>Email Verification</h2>
            <p>Hi {username},</p>
            <p>Use this link to reset your password</p>
            <div style='text-align: center; margin: 30px 0;'>
                <a href='{callbackUrl}' style='background-color: #007bff; color: white; padding: 12px 25px; text-decoration: none; border-radius: 4px; display: inline-block;'>Verify Email</a>
            </div>

            <h2 style='color: #333; margin-top: 30px;'>Support Our Mission</h2>
            <p>If you'd like to support this project, you can contribute through:</p>
            <div style='text-align: center; margin: 20px 0;'>
                <a href='https://patreon.com/VladyslavYatsuta?utm_medium=unknown&utm_source=join_link&utm_campaign=creatorshare_creator&utm_content=copyLink' 
                   style='display: inline-block; margin: 10px; padding: 10px 20px; background-color: #FF424D; color: white; text-decoration: none; border-radius: 4px;'>
                    Support on Patreon
                </a>
                <a href='https://www.buymeacoffee.com/vladyslavyatsuta' 
                   style='display: inline-block; margin: 10px; padding: 10px 20px; background-color: #FFDD00; color: black; text-decoration: none; border-radius: 4px;'>
                    Buy me a coffee ☕
                </a>
                <a href='https://www.venmo.com/u/textynews_us' 
                   style='display: inline-block; margin: 10px; padding: 10px 20px; background-color: #008CFF; color: white; text-decoration: none; border-radius: 4px;'>
                    Support via Venmo
                </a>
            </div>

            <h2 style='color: #333; margin-top: 30px;'>Contact Us</h2>
            <p>We value your feedback and are constantly working to improve our service. Reach out to us at:</p>
            <p>Email: <a href='mailto:support@contiguity.xyz' style='color: #007bff;'>support@contiguity.xyz</a></p>
            <p>Website: <a href='https://contiguity.xyz' style='color: #007bff;'>https://contiguity.xyz</a></p>

            <p style='margin-top: 30px;'>Best regards,<br>The Contiguity Team</p>
        </div>";
    }
    public static string GetConfirmationCodeHtmlPart(string username, string code)
    {
return $@"
    <div style='font-family: -apple-system, BlinkMacSystemFont, ""Segoe UI"", Roboto, Helvetica, Arial, sans-serif; max-width: 600px; margin: 0 auto; padding: 40px 20px; background-color: #ffffff;'>
        
        <!-- Header -->
        <div style='text-align: center; margin-bottom: 40px;'>
            <h1 style='color: #1a1a1a; font-size: 28px; font-weight: 600; margin: 0 0 10px 0;'>Welcome to Dreamify</h1>
            <p style='color: #666; font-size: 16px; margin: 0;'>Verify your email to get started</p>
        </div>

        <!-- Greeting -->
        <p style='color: #333; font-size: 16px; line-height: 1.6; margin-bottom: 20px;'>Hi {username},</p>
        
        <p style='color: #666; font-size: 15px; line-height: 1.6; margin-bottom: 30px;'>
            Thank you for joining Dreamify! Please enter the verification code below in the app to confirm your email address.
        </p>
        
        <!-- Verification Code Box -->
        <div style='text-align: center; margin: 40px 0;'>
            <div style='background: linear-gradient(135deg, #667eea 0%, #764ba2 100%); border-radius: 12px; padding: 30px; display: inline-block; box-shadow: 0 4px 15px rgba(102, 126, 234, 0.4);'>
                <p style='color: #ffffff; font-size: 14px; font-weight: 500; margin: 0 0 10px 0; letter-spacing: 1px; text-transform: uppercase;'>Your Verification Code</p>
                <span style='font-size: 36px; font-weight: 700; letter-spacing: 10px; color: #ffffff; font-family: ""Courier New"", monospace;'>{code}</span>
            </div>
        </div>
        
        <p style='text-align: center; color: #999; font-size: 13px; margin: 0 0 50px 0;'>
            ⏱ This code expires in 10 minutes
        </p>

        <!-- Divider -->
        <div style='border-top: 1px solid #e5e5e5; margin: 50px 0 40px 0;'></div>

        <!-- Support Section -->
        <div style='text-align: center; margin-bottom: 40px;'>
            <p style='color: #666; font-size: 15px; margin: 0 0 20px 0;'>
                Enjoying Dreamify? Support the project! ☕
            </p>
            <a href='https://www.buymeacoffee.com/vladyslavyatsuta' 
               style='display: inline-block; padding: 14px 32px; background-color: #FFDD00; color: #000000; text-decoration: none; border-radius: 8px; font-weight: 600; font-size: 15px; box-shadow: 0 2px 10px rgba(255, 221, 0, 0.3); transition: transform 0.2s;'>
                ☕ Buy Me a Coffee
            </a>
        </div>

        <!-- Footer -->
        <div style='border-top: 1px solid #e5e5e5; padding-top: 30px; margin-top: 40px;'>
            <p style='color: #999; font-size: 13px; text-align: center; margin: 0 0 10px 0;'>
                Need help? Contact us at <a href='mailto:support@dream-ify.com' style='color: #667eea; text-decoration: none;'>support@dream-ify.com</a>
            </p>
            <p style='color: #999; font-size: 13px; text-align: center; margin: 0;'>
                © 2025 Dreamify. All rights reserved.
            </p>
        </div>

    </div>";
    }
    
    
    
    
}