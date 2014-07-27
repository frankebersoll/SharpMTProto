﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TestData.cs">
//   Copyright (c) 2013 Alexander Logger. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System;
using BigMath.Utils;
using SharpMTProto.Authentication;
using SharpMTProto.Services;
using SharpTL;

namespace SharpMTProto.Tests
{
    public static class TestData
    {
        public static readonly PublicKey[] TestPublicKeys =
        {
            new PublicKey(
                "c6aeda78b02a251db4b6441031f467fa871faed32526c436524b1fb3b5dca28efb8c089dd1b46d92c895993d87108254951c5f001a0f055f3063dcd14d431a300eb9e29517e359a1c9537e5e87ab1b116faecf5d17546ebc21db234d9d336a693efcb2b6fbcca1e7d1a0be414dca408a11609b9c4269a920b09fed1f9a1597be02761430f09e4bc48fcafbe289054c99dba51b6b5eb7d9c3a2ab4e490545b4676bd620e93804bcac93bf94f73f92c729ca899477ff17625ef14a934d51dc11d5f8650a3364586b3a52fcff2fedec8a8406cac4e751705a472e55707e3c8cd5594342b119c6c3293532d85dbe9271ed54a2fd18b4dc79c04a30951107d5639397",
                "010001", 0x9a996a1db11c729bUL),
            new PublicKey(
                "b1066749655935f0a5936f517034c943bea7f3365a8931ae52c8bcb14856f004b83d26cf2839be0f22607470d67481771c1ce5ec31de16b20bbaa4ecd2f7d2ecf6b6356f27501c226984263edc046b89fb6d3981546b01d7bd34fedcfcc1058e2d494bda732ff813e50e1c6ae249890b225f82b22b1e55fcb063dc3c0e18e91c28d0c4aa627dec8353eee6038a95a4fd1ca984eb09f94aeb7a2220635a8ceb450ea7e61d915cdb4eecedaa083aa3801daf071855ec1fb38516cb6c2996d2d60c0ecbcfa57e4cf1fb0ed39b2f37e94ab4202ecf595e167b3ca62669a6da520859fb6d6c6203dfdfc79c75ec3ee97da8774b2da903e3435f2cd294670a75a526c1",
                "010001", 0xb05b2a6f70cdea78UL),
            new PublicKey(
                "c150023e2f70db7985ded064759cfecf0af328e69a41daf4d6f01b538135a6f91f8f8b2a0ec9ba9720ce352efcf6c5680ffc424bd634864902de0b4bd6d49f4e580230e3ae97d95c8b19442b3c0a10d8f5633fecedd6926a7f6dab0ddb7d457f9ea81b8465fcd6fffeed114011df91c059caedaf97625f6c96ecc74725556934ef781d866b34f011fce4d835a090196e9a5f0e4449af7eb697ddb9076494ca5f81104a305b6dd27665722c46b60e5df680fb16b210607ef217652e60236c255f6a28315f4083a96791d7214bf64c1df4fd0db1944fb26a2a57031b32eee64ad15a8ba68885cde74a5bfc920f6abf59ba5c75506373e7130f9042da922179251f",
                "010001", 0xc3b42b026ce86b21UL),
            new PublicKey(
                "c2a8c55b4a62e2b78a19b91cf692bcdc4ba7c23fe4d06f194e2a0c30f6d9996f7d1a2bcc89bc1ac4333d44359a6c433252d1a8402d9970378b5912b75bc8cc3fa76710a025bcb9032df0b87d7607cc53b928712a174ea2a80a8176623588119d42ffce40205c6d72160860d8d80b22a8b8651907cf388effbef29cd7cf2b4eb8a872052da1351cfe7fec214ce48304ea472bd66329d60115b3420d08f6894b0410b6ab9450249967617670c932f7cbdb5d6fbcce1e492c595f483109999b2661fcdeec31b196429b7834c7211a93c6789d9ee601c18c39e521fda9d7264e61e518add6f0712d2d5228204b851e13c4f322e5c5431c3b7f31089668486aadc59f",
                "010001", 0x71e025b6c76033e3UL)
        };

        public static readonly byte[] ReqPQ = "00000000000000004A967027C47AE55114000000789746603E0549828CCA27E966B301A48FECE2FC".HexToBytes();

        public static readonly byte[] ResPQ =
            "000000000000000001C8831EC97AE55140000000632416053E0549828CCA27E966B301A48FECE2FCA5CF4D33F4A11EA877BA4AA5739073300817ED48941A08F98100000015C4B51C01000000216BE86C022BB4C3"
                .HexToBytes();

        public static readonly byte[] ReqDHParams =
            "0000000000000000277A7117C97AE55140010000BEE412D73E0549828CCA27E966B301A48FECE2FCA5CF4D33F4A11EA877BA4AA57390733004494C553B0000000453911073000000216BE86C022BB4C3FE0001007BB0100A523161904D9C69FA04BC60DECFC5DD74B99995C768EB60D8716E2109BAF2D4601DAB6B09610DC11067BB89021E09471FCFA52DBD0F23204AD8CA8B012BF40A112F44695AB6C266955386114EF5211E6372227ADBD34995D3E0E5FF02EC63A43F9926878962F7C570E6A6E78BF8366AF917A5272675C46064BE62E3E202EFA8B1ADFB1C32A898C2987BE27B5F31D57C9BB963ABCB734B16F652CEDB4293CBB7C878A3A3FFAC9DBEA9DF7C67BC9E9508E111C78FC46E057F5C65ADE381D91FEE430A6B576A99BDF8551FDB1BE2B57069B1A45730618F27427E8A04720B4971EF4A9215983D68F2830C3EAA6E40385562F970D38A05C9F1246DC33438E6"
                .HexToBytes();

        public static readonly byte[] ServerDHParams =
            "000000000000000001544336CB7AE551780200005C07E8D03E0549828CCA27E966B301A48FECE2FCA5CF4D33F4A11EA877BA4AA573907330FE50020028A92FE20173B347A8BB324B5FAB2667C9A8BBCE6468D5B509A4CBDDC186240AC912CF7006AF8926DE606A2E74C0493CAA57741E6C82451F54D3E068F5CCC49B4444124B9666FFB405AAB564A3D01E67F6E912867C8D20D9882707DC330B17B4E0DD57CB53BFAAFA9EF5BE76AE6C1B9B6C51E2D6502A47C883095C46C81E3BE25F62427B585488BB3BF239213BF48EB8FE34C9A026CC8413934043974DB03556633038392CECB51F94824E140B98637730A4BE79A8F9DAFA39BAE81E1095849EA4C83467C92A3A17D997817C8A7AC61C3FF414DA37B7D66E949C0AEC858F048224210FCC61F11C3A910B431CCBD104CCCC8DC6D29D4A5D133BE639A4C32BBFF153E63ACA3AC52F2E4709B8AE01844B142C1EE89D075D64F69A399FEB04E656FE3675A6F8F412078F3D0B58DA15311C1A9F8E53B3CD6BB5572C294904B726D0BE337E2E21977DA26DD6E33270251C2CA29DFCC70227F0755F84CFDA9AC4B8DD5F84F1D1EB36BA45CDDC70444D8C213E4BD8F63B8AB95A2D0B4180DC91283DC063ACFB92D6A4E407CDE7C8C69689F77A007441D4A6A8384B666502D9B77FC68B5B43CC607E60A146223E110FCB43BC3C942EF981930CDC4A1D310C0B64D5E55D308D863251AB90502C3E46CC599E886A927CDA963B9EB16CE62603B68529EE98F9F5206419E03FB458EC4BD9454AA8F6BA777573CC54B328895B1DF25EAD9FB4CD5198EE022B2B81F388D281D5E5BC580107CA01A50665C32B552715F335FD76264FAD00DDD5AE45B94832AC79CE7C511D194BC42B70EFA850BB15C2012C5215CABFE97CE66B8D8734D0EE759A638AF013"
                .HexToBytes();

        public static readonly byte[] AuthKey =
            "AB96E207C631300986F30EF97DF55E179E63C112675F0CE502EE76D74BBEE6CBD1E95772818881E9F2FF54BD52C258787474F6A7BEA61EABE49D1D01D55F64FC07BC31685716EC8FB46FEACF9502E42CFD6B9F45A08E90AA5C2B5933AC767CBE1CD50D8E64F89727CA4A1A5D32C0DB80A9FCDBDDD4F8D5A1E774198F1A4299F927C484FEEC395F29647E43C3243986F93609E23538C21871DF50E00070B3B6A8FA9BC15628E8B43FF977409A61CEEC5A21CF7DFB5A4CC28F5257BC30CD8F2FB92FBF21E28924065F50E0BBD5E11A420300E2C136B80E9826C6C5609B5371B7850AA628323B6422F3A94F6DFDE4C3DC1EA60F7E11EE63122B3F39CBD1A8430157"
                .HexToBytes();

        public static readonly byte[] EncryptedData =
            "7BB0100A523161904D9C69FA04BC60DECFC5DD74B99995C768EB60D8716E2109BAF2D4601DAB6B09610DC11067BB89021E09471FCFA52DBD0F23204AD8CA8B012BF40A112F44695AB6C266955386114EF5211E6372227ADBD34995D3E0E5FF02EC63A43F9926878962F7C570E6A6E78BF8366AF917A5272675C46064BE62E3E202EFA8B1ADFB1C32A898C2987BE27B5F31D57C9BB963ABCB734B16F652CEDB4293CBB7C878A3A3FFAC9DBEA9DF7C67BC9E9508E111C78FC46E057F5C65ADE381D91FEE430A6B576A99BDF8551FDB1BE2B57069B1A45730618F27427E8A04720B4971EF4A9215983D68F2830C3EAA6E40385562F970D38A05C9F1246DC33438E6"
                .HexToBytes();

        public static readonly byte[] ServerDHParamsOkEncryptedAnswer =
            "28A92FE20173B347A8BB324B5FAB2667C9A8BBCE6468D5B509A4CBDDC186240AC912CF7006AF8926DE606A2E74C0493CAA57741E6C82451F54D3E068F5CCC49B4444124B9666FFB405AAB564A3D01E67F6E912867C8D20D9882707DC330B17B4E0DD57CB53BFAAFA9EF5BE76AE6C1B9B6C51E2D6502A47C883095C46C81E3BE25F62427B585488BB3BF239213BF48EB8FE34C9A026CC8413934043974DB03556633038392CECB51F94824E140B98637730A4BE79A8F9DAFA39BAE81E1095849EA4C83467C92A3A17D997817C8A7AC61C3FF414DA37B7D66E949C0AEC858F048224210FCC61F11C3A910B431CCBD104CCCC8DC6D29D4A5D133BE639A4C32BBFF153E63ACA3AC52F2E4709B8AE01844B142C1EE89D075D64F69A399FEB04E656FE3675A6F8F412078F3D0B58DA15311C1A9F8E53B3CD6BB5572C294904B726D0BE337E2E21977DA26DD6E33270251C2CA29DFCC70227F0755F84CFDA9AC4B8DD5F84F1D1EB36BA45CDDC70444D8C213E4BD8F63B8AB95A2D0B4180DC91283DC063ACFB92D6A4E407CDE7C8C69689F77A007441D4A6A8384B666502D9B77FC68B5B43CC607E60A146223E110FCB43BC3C942EF981930CDC4A1D310C0B64D5E55D308D863251AB90502C3E46CC599E886A927CDA963B9EB16CE62603B68529EE98F9F5206419E03FB458EC4BD9454AA8F6BA777573CC54B328895B1DF25EAD9FB4CD5198EE022B2B81F388D281D5E5BC580107CA01A50665C32B552715F335FD76264FAD00DDD5AE45B94832AC79CE7C511D194BC42B70EFA850BB15C2012C5215CABFE97CE66B8D8734D0EE759A638AF013"
                .HexToBytes();

        public static readonly byte[] ServerDHInnerDataWithHash =
            "4B0AF668CF60A358233F93B7341FCA7E7F02A8C2BA0D89B53E0549828CCA27E966B301A48FECE2FCA5CF4D33F4A11EA877BA4AA57390733002000000FE000100C71CAEB9C6B1C9048E6C522F70F13F73980D40238E3E21C14934D037563D930F48198A0AA7C14058229493D22530F4DBFA336F6E0AC925139543AED44CCE7C3720FD51F69458705AC68CD4FE6B6B13ABDC9746512969328454F18FAF8C595F642477FE96BB2A941D5BCD1D4AC8CC49880708FA9B378E3C4F3A9060BEE67CF9A4A4A695811051907E162753B56B0F6B410DBA74D8A84B2A14B3144E0EF1284754FD17ED950D5965B4B9DD46582DB1178D169C6BC465B0D6FF9CA3928FEF5B9AE4E418FC15E83EBEA0F87FA9FF5EED70050DED2849F47BF959D956850CE929851F0D8115F635B105EE2E4E15D04B2454BF6F4FADF034B10403119CD8E3B92FCC5BFE000100262AABA621CC4DF587DC94CF8252258C0B9337DFB47545A49CDD5C9B8EAE7236C6CADC40B24E88590F1CC2CC762EBF1CF11DCC0B393CAAD6CEE4EE5848001C73ACBB1D127E4CB93072AA3D1C8151B6FB6AA6124B7CD782EAF981BDCFCE9D7A00E423BD9D194E8AF78EF6501F415522E44522281C79D906DDB79C72E9C63D83FB2A940FF779DFB5F2FD786FB4AD71C9F08CF48758E534E9815F634F1E3A80A5E1C2AF210C5AB762755AD4B2126DFA61A77FA9DA967D65DFD0AFB5CDF26C4D4E1A88B180F4E0D0B45BA1484F95CB2712B50BF3F5968D9D55C99C0FB9FB67BFF56D7D4481B634514FBA3488C4CDA2FC0659990E8E868B28632875A9AA703BCDCE8FCB7AE55199E2DDDD536648D8"
                .HexToBytes();

        public static readonly byte[] SetClientDHParamsEncryptedData =
            "928A4957D0463B525C1CC48AABAA030A256BE5C746792C84CA4C5A0DF60AC799048D98A38A8480EDCF082214DFC79DCB9EE34E206513E2B3BC1504CFE6C9ADA46BF9A03CA74F192EAF8C278454ADABC795A566615462D31817382984039505F71CB33A41E2527A4B1AC05107872FED8E3ABCEE1518AE965B0ED3AED7F67479155BDA8E4C286B64CDF123EC748CF289B1DB02D1907B562DF462D8582BA6F0A3022DC2D3504D69D1BA48B677E3A830BFAFD67584C8AA24E1344A8904E305F9587C92EF964F0083F50F61EAB4A393EAA33C9270294AEDC7732891D4EA1599F52311D74469D2112F4EDF3F342E93C8E87E812DC3989BAECFE6740A46077524C75093F5A5405736DE8937BB6E42C9A0DCF22CA53227D462BCCC2CFE94B6FE86AB7FBFA395021F66661AF7C0024CA2986CA03F3476905407D1EA9C010B763258DB1AA2CC7826D91334EFC1FDC665B67FE45ED0"
                .HexToBytes();

        public static readonly byte[] SetClientDHParams =
            "00000000000000006D2CA32ACD7AE551780100001F5F04F53E0549828CCA27E966B301A48FECE2FCA5CF4D33F4A11EA877BA4AA573907330FE500100928A4957D0463B525C1CC48AABAA030A256BE5C746792C84CA4C5A0DF60AC799048D98A38A8480EDCF082214DFC79DCB9EE34E206513E2B3BC1504CFE6C9ADA46BF9A03CA74F192EAF8C278454ADABC795A566615462D31817382984039505F71CB33A41E2527A4B1AC05107872FED8E3ABCEE1518AE965B0ED3AED7F67479155BDA8E4C286B64CDF123EC748CF289B1DB02D1907B562DF462D8582BA6F0A3022DC2D3504D69D1BA48B677E3A830BFAFD67584C8AA24E1344A8904E305F9587C92EF964F0083F50F61EAB4A393EAA33C9270294AEDC7732891D4EA1599F52311D74469D2112F4EDF3F342E93C8E87E812DC3989BAECFE6740A46077524C75093F5A5405736DE8937BB6E42C9A0DCF22CA53227D462BCCC2CFE94B6FE86AB7FBFA395021F66661AF7C0024CA2986CA03F3476905407D1EA9C010B763258DB1AA2CC7826D91334EFC1FDC665B67FE45ED0"
                .HexToBytes();

        public static readonly byte[] DhGenOk =
            "00000000000000000130AAC5CE7AE5513400000034F7CB3B3E0549828CCA27E966B301A48FECE2FCA5CF4D33F4A11EA877BA4AA573907330CCEBC0217266E1EDEC7FB0A0EED6C220".HexToBytes();

        public static readonly byte[] ClientDHInnerDataWithHash =
            "2DBA88AD57505EAF011C1E76D5AD7180C463C32D54B643663E0549828CCA27E966B301A48FECE2FCA5CF4D33F4A11EA877BA4AA5739073300000000000000000FE00010073700E7BFC7AEEC828EB8E0DCC04D09A0DD56A1B4B35F72F0B55FCE7DB7EBB72D7C33C5D4AA59E1C74D09B01AE536B318CFED436AFDB15FE9EB4C70D7F0CB14E46DBBDE9053A64304361EB358A9BB32E9D5C2843FE87248B89C3F066A7D5876D61657ACC52B0D81CD683B2A0FA93E8ADAB20377877F3BC3369BBF57B10F5B589E65A9C27490F30A0C70FFCFD3453F5B379C1B9727A573CFFDCA8D23C721B135B92E529B1CDD2F7ABD4F34DAC4BE1EEAF60993DDE8ED45890E4F47C26F2C0B2E037BB502739C8824F2A99E2B1E7E416583417CC79A8807A4BDAC6A5E9805D4F6186C37D66F6988C9F9C752896F3D34D25529263FAF2670A09B2A59CE35264511F000000000000000000000000"
                .HexToBytes();

        public static readonly byte[] TmpAesKey = "F011280887C7BB01DF0FC4E17830E0B91FBB8BE4B2267CB985AE25F33B527253".HexToBytes();

        public static readonly byte[] TmpAesIV = "3212D579EE35452ED23E0D0C92841AA7D31B2E9BDEF2151E80D15860311C85DB".HexToBytes();

        public static readonly byte[] GB =
            "73700E7BFC7AEEC828EB8E0DCC04D09A0DD56A1B4B35F72F0B55FCE7DB7EBB72D7C33C5D4AA59E1C74D09B01AE536B318CFED436AFDB15FE9EB4C70D7F0CB14E46DBBDE9053A64304361EB358A9BB32E9D5C2843FE87248B89C3F066A7D5876D61657ACC52B0D81CD683B2A0FA93E8ADAB20377877F3BC3369BBF57B10F5B589E65A9C27490F30A0C70FFCFD3453F5B379C1B9727A573CFFDCA8D23C721B135B92E529B1CDD2F7ABD4F34DAC4BE1EEAF60993DDE8ED45890E4F47C26F2C0B2E037BB502739C8824F2A99E2B1E7E416583417CC79A8807A4BDAC6A5E9805D4F6186C37D66F6988C9F9C752896F3D34D25529263FAF2670A09B2A59CE35264511F"
                .HexToBytes();

        public static readonly byte[] B =
            "6F620AFA575C9233EB4C014110A7BCAF49464F798A18A0981FEA1E05E8DA67D9681E0FD6DF0EDF0272AE3492451A84502F2EFC0DA18741A5FB80BD82296919A70FAA6D07CBBBCA2037EA7D3E327B61D585ED3373EE0553A91CBD29B01FA9A89D479CA53D57BDE3A76FBD922A923A0A38B922C1D0701F53FF52D7EA9217080163A64901E766EB6A0F20BC391B64B9D1DD2CD13A7D0C946A3A7DF8CEC9E2236446F646C42CFE2B60A2A8D776E56C8D7519B08B88ED0970E10D12A8C9E355D765F2B7BBB7B4CA9360083435523CB0D57D2B106FD14F94B4EEE79D8AC131CA56AD389C84FE279716F8124A543337FB9EA3D988EC5FA63D90A4BA3970E7A39E5C0DE5"
                .HexToBytes();

        public static readonly byte[] G = "00000002".HexToBytes();

        public static readonly byte[] GA =
            "262AABA621CC4DF587DC94CF8252258C0B9337DFB47545A49CDD5C9B8EAE7236C6CADC40B24E88590F1CC2CC762EBF1CF11DCC0B393CAAD6CEE4EE5848001C73ACBB1D127E4CB93072AA3D1C8151B6FB6AA6124B7CD782EAF981BDCFCE9D7A00E423BD9D194E8AF78EF6501F415522E44522281C79D906DDB79C72E9C63D83FB2A940FF779DFB5F2FD786FB4AD71C9F08CF48758E534E9815F634F1E3A80A5E1C2AF210C5AB762755AD4B2126DFA61A77FA9DA967D65DFD0AFB5CDF26C4D4E1A88B180F4E0D0B45BA1484F95CB2712B50BF3F5968D9D55C99C0FB9FB67BFF56D7D4481B634514FBA3488C4CDA2FC0659990E8E868B28632875A9AA703BCDCE8F"
                .HexToBytes();

        public static readonly byte[] P =
            "C71CAEB9C6B1C9048E6C522F70F13F73980D40238E3E21C14934D037563D930F48198A0AA7C14058229493D22530F4DBFA336F6E0AC925139543AED44CCE7C3720FD51F69458705AC68CD4FE6B6B13ABDC9746512969328454F18FAF8C595F642477FE96BB2A941D5BCD1D4AC8CC49880708FA9B378E3C4F3A9060BEE67CF9A4A4A695811051907E162753B56B0F6B410DBA74D8A84B2A14B3144E0EF1284754FD17ED950D5965B4B9DD46582DB1178D169C6BC465B0D6FF9CA3928FEF5B9AE4E418FC15E83EBEA0F87FA9FF5EED70050DED2849F47BF959D956850CE929851F0D8115F635B105EE2E4E15D04B2454BF6F4FADF034B10403119CD8E3B92FCC5B"
                .HexToBytes();

        public static readonly UInt64 InitialSalt = (0xA5CF4D33F4A11EA8 ^ 0x311C85DB234AA264).ToBytes().ToUInt64(asLittleEndian: false);
    }

    public class TestNonceGenerator : INonceGenerator, IDisposable
    {
        private static readonly byte[] Nonces =
            ("3E0549828CCA27E966B301A48FECE2FC" + "311C85DB234AA2640AFC4A76A735CF5B1F0FD68BD17FA181E1229AD867CC024D" +
                "6F620AFA575C9233EB4C014110A7BCAF49464F798A18A0981FEA1E05E8DA67D9681E0FD6DF0EDF0272AE3492451A84502F2EFC0DA18741A5FB80BD82296919A70FAA6D07CBBBCA2037EA7D3E327B61D585ED3373EE0553A91CBD29B01FA9A89D479CA53D57BDE3A76FBD922A923A0A38B922C1D0701F53FF52D7EA9217080163A64901E766EB6A0F20BC391B64B9D1DD2CD13A7D0C946A3A7DF8CEC9E2236446F646C42CFE2B60A2A8D776E56C8D7519B08B88ED0970E10D12A8C9E355D765F2B7BBB7B4CA9360083435523CB0D57D2B106FD14F94B4EEE79D8AC131CA56AD389C84FE279716F8124A543337FB9EA3D988EC5FA63D90A4BA3970E7A39E5C0DE5")
                .HexToBytes();

        private TLStreamer _nonceStream = new TLStreamer(Nonces);

        public void Dispose()
        {
            if (_nonceStream != null)
            {
                _nonceStream.Dispose();
                _nonceStream = null;
            }
        }

        public byte[] GetNonce(uint length)
        {
            return _nonceStream.ReadBytes((int) length);
        }
    }
}
