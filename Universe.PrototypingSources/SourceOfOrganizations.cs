namespace Universe.PrototypingSources
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Text;

    public class SourceOfOrganizations
    {
        public static readonly List<OrganizationInfo> Orgs = Parse();
        private static Dictionary<string, OrganizationInfo> Parents;

        public static List<OrganizationInfo> GetParents(string id)
        {
            List<OrganizationInfo> ret = new List<OrganizationInfo>();
            OrganizationInfo parent = null;
            Parents.TryGetValue(id, out parent);
            while (parent != null)
            {
                ret.Add(parent);
                Parents.TryGetValue(parent.Id, out parent);
            }

            ret.Reverse();
            return ret;
        }

        public static string GetParentsAsString(string id)
        {
            var parents = GetParents(id);
            StringBuilder ret = new StringBuilder();
            foreach (var org in parents)
            {
                ret
                    .Append(ret.Length == 0 ? "" : " --> ")
                    .Append("#")
                    .Append(org.Id)
                    .Append(" '")
                    .Append(org.Name)
                    .Append("'");
            }

            return ret.ToString();
        }

        private static List<OrganizationInfo> Parse()
        {
            List<OrganizationInfo> ret = new List<OrganizationInfo>(360);
            string[] lines = Raw.Split('\r', '\n');
            int[] indexes = new int[4];
            foreach (string line in lines)
            {
                var copy = line.Trim();
                if (copy.Length == 0) continue;

                int preSpaces = 0;
                foreach (var ch in line)
                {
                    if (ch != ' ') break;
                    preSpaces++;
                }

                
                int level = -1;
                if (preSpaces == 0) level = 0;
                else if (preSpaces == 2) level = 1;
                else if (preSpaces == 4) level = 2;
                else if (preSpaces == 8) level = 3;
                else throw new InvalidOperationException("Raw Organizations tree is corrupted");

                indexes[level]++;
                StringBuilder id = new StringBuilder();
                for (int i = 0; i <= level; i++)
                    id.Append(Counter2Id(indexes[i]));

                StringBuilder idParent = new StringBuilder();
                for (int i = 0; i < level; i++)
                    idParent.Append(Counter2Id(indexes[i]));

                Debug.WriteLine(string.Format("{0} ({1}): {2}", id, level, copy));
                OrganizationInfo org = new OrganizationInfo();
                org.Id = id.ToString();
                org.IdParent = idParent.Length == 0 ? null : idParent.ToString();
                org.Name = copy.TrimEnd(';');
                ret.Add(org);
            }

            // IsLeaf & IsGroup
            Parents = new Dictionary<string, OrganizationInfo>();
            foreach (var org in ret)
            {
                org.IsLeaf = true;
                string prefix = org.Id + "/";
                foreach (var other in ret)
                {
                    if (other.Id.StartsWith(prefix))
                    {
                        org.IsLeaf = false;
                        other.IsGroup = true;
                        break;
                    }
                }

                foreach (var other in ret)
                {
                    if (other.Id == org.IdParent)
                    {
                        Parents[org.Id] = other;
                        break;
                    }
                }

            }

            return ret;
        }

        static string Counter2Id(int counter)
        {
            return "/" + ((char) (65 + (counter - 1) / 10)) + ((counter - 1) % 10);
        }


        // WARNING: Relationships are defined by number of spaces before a name
        //          The supported numers are 0, 2, 4 and 8.
        private const string Raw = @"
United Nations and agencies
  The UN six principal organs

    The General Assembly (the main deliberative assembly);
    The Security Council (decides certain resolutions for peace and security);
    The Economic and Social Council (assists in promoting international economic and social cooperation and development);
    The Secretariat (provides studies, information, and facilities needed by the UN);
    The International Court of Justice (the primary judicial organ).
    The United Nations Trusteeship Council (inactive)

  The UN various Funds, Programmes and specialized agencies

    Food and Agriculture Organization
    International Labour Organization
    International Civil Aviation Organization
    International Maritime Organization
    Joint United Nations Programme on HIV/AIDS
    United Nations Capital Development Fund
    United Nations International Children's Emergency Fund (UNICEF)
    United Nations Development Programme
    United Nations Educational, Scientific and Cultural Organization (UNESCO)
    United Nations Environment Programme
    United Nations Human Settlements Programme
    United Nations Industrial Development Organization
    United Nations Office for Disaster Risk Reduction (UNISDR)
    United Nations Office on Drugs and Crime
    Universal Postal Union
    World Health Organization (WHO)
    World Intellectual Property Organization
    World Food Programme
    World Meteorological Organization
    World Tourism Organization

  The UN various offices
    United Nations Headquarters (New York City)
    United Nations Office at Geneva
    United Nations Office at Nairobi
    United Nations Office at Vienna

  The UN subsidiary organs

    International Criminal Tribunal for the former Yugoslavia (ICTY)
    International Criminal Tribunal for Rwanda (ICTR)

Environmental organizations

  Agreement on the Conservation of Albatrosses and Petrels (ACAP)
  Global Environment Facility (GEF)
  Intergovernmental Panel on Climate Change (IPCC)
  The International Union for Conservation of Nature (IUCN)
  Partnerships in Environmental Management for the Seas of East Asia (PEMSEA)
  United Nations Environment Programme (UNEP)

Fisheries organizations

  Asia-Pacific Fishery Commission (APFIC)
  Commission for the Conservation of Antarctic Marine Living Resources (CCAMLR)
  Great Lakes Fishery Commission (GLFC)
  Indian Ocean Tuna Commission (IOTC)
  Inter-American Tropical Tuna Commission (IATTC)
  International Commission for the Conservation of Atlantic Tunas (ICCAT)
  International Pacific Halibut Commission (IPHC)
  International Whaling Commission (IWC)
  North-East Atlantic Fisheries Commission (NEAFC)
  Northwest Atlantic Fisheries Organization (NAFO)
  North Atlantic Salmon Conservation Organization (NASCO)
  Pacific Salmon Commission (PSC)
  Southeast Asian Fisheries Development Center (SEAFDEC)
  Western and Central Pacific Fisheries Commission (WCPFC)

Migration organizations

  International Organization for Migration (IOM)
  International Centre for Migration Policy Development (ICMPD)

Maritime organizations

  Antarctic Treaty Secretariat (ATS)
  International Hydrographic Organization
  International Maritime Organization
  International Seabed Authority
  International Council for the Exploration of the Sea (ICES)
  North Pacific Marine Science Organization (PICES)

Arms and weapon of mass destruction control

  Organization for Security and Co-operation in Europe (OSCE)
  Conference on Disarmament
  Organisation for the Prohibition of Chemical Weapons (OPCW)
  Preparatory Commission for the Comprehensive Nuclear-Test-Ban Treaty Organization (CTBTO)
  Wassenaar Arrangement
  Nuclear Suppliers Group (NSG)
  Australia Group (AG)
  Missile Technology Control Regime (MTCR)

Missing Persons

  International Commission on Missing Persons (ICMP)

Energy organizations
  Multi sector organizations

    International Energy Agency
    Energy Charter
    Energy Community
    United Nations Industrial Development Organization (UNIDO)
    International Institute for Applied Systems Analysis (IIASA)
    International Energy Forum (IEF)

  Sustainable energy organizations

    International Renewable Energy Agency (IRENA)
    Sustainable Energy for All (SE4ALL)
    Renewable Energy and Energy Efficiency Partnership (REEEP)
    International Solar Alliance

  Nuclear power organizations

    European Atomic Energy Community
    International Atomic Energy Agency
    International Centre for Synchrotron-Light for Experimental Science Applications in the Middle East
    Korean Peninsula Energy Development Organization
    Nuclear Energy Agency
    United Nations Atomic Energy Commission
    World Association of Nuclear Operators

Financial, trade, and customs organizations

  Alliance for Financial Inclusion (AFI)
  African Development Bank
  Asian Development Bank
  Asian Infrastructure Investment Bank
  Bank for International Settlements
  Black Sea Trade and Development Bank (BSTDB)
  Caribbean Development Bank (CDB)
  Council of Europe Development Bank (CEB)
  European Bank for Reconstruction and Development (EBRD)
  Inter-American Development Bank
  International Bureau of Weights and Measures (BIPM)
  International Energy Agency (IEA)
  International Fund for Agricultural Development (IFAD)
  International Development Law Organization (IDLO), headquartered in Rome
  International Monetary Fund (IMF)
  Islamic Development Bank (IDB)
  Netherlands Development Finance Company (FMO)
  Nordic Development Fund (NDF)
  Nordic Investment Bank (NIB)
  New Development Bank (NDB)
  OPEC Fund for International Development (OPEC Fund)
  Organisation for Economic Co-operation and Development (OECD)
  Organization of Petroleum-Exporting Countries (OPEC)
  West African Development Bank (BOAD)
  World Bank Group
    International Bank for Reconstruction and Development (IBRD)
    International Development Association (IDA)
    International Finance Corporation (IFC)
    Multilateral Investment Guarantee Agency (MIGA)
    International Centre for Settlement of Investment Disputes (ICSID)
  World Customs Organization (WCO)
  World Trade Organization (WTO)

Law enforcement cooperation

  International Criminal Court (ICC)
  International Criminal Police Organization (Interpol)

Regional organizations

  Europe

    European Union (EU)

        Western European Union (defunct)

    Council of Europe (CoE)
    Energy Community
    European Free Trade Association (EFTA)
    European Patent Organisation (EPO)
    European Science Foundation
    European Organisation for the Safety of Air Navigation (EUROCONTROL)
    Group of 9 (G9)
    International Commission on Civil Status (ICCS)
    Central Commission for Navigation on the Rhine (CCNR)
    Council of the Baltic Sea States (CBSS)
    Baltic Black Sea Union (BBSU)
    Assembly of European Regions (AER)
    Eiroforum
        CERN
        European Fusion Development Agreement (EFDA JET)
        European Molecular Biology Laboratory (EMBL)
        European Space Agency (ESA)
        European Organisation for the Exploitation of Meteorological Satellites (EUMETSAT)
        European Southern Observatory (ESO)
        European Synchrotron Radiation Facility (ESRF)
        European x-ray free electron laser (European XFEL)
        Institut Laue–Langevin (ILL)
    Baltic Marine Environment Protection Commission
    Benelux
    Belgium–Luxembourg Economic Union
    British–Irish Council
    Nordic Council
    Nordic Investment Bank
    Northern Dimension Partnership in Public Health and Social Well-being (NDPHS)
    Organisation for Joint Armament Cooperation(OCCAR)
    Agency for International Trade Information and Cooperation (AITIC)
    Visegrád Group (V4)
    EUREKA
    European Cooperation in Science and Technology (COST)
    Community for Democracy and Rights of Nations (Commonwealth of Unrecognized States)
    European Centre for Medium-range Weather Forecasts (ECMWF)
    West Nordic Council

  Asia

    Asia Cooperation Dialogue (ACD)
    Asian Development Bank (ADB)
    East Asia Summit (EAS)
    Association of Southeast Asian Nations (ASEAN)
    Bay of Bengal Initiative for Multi-Sectoral Technical and Economic Cooperation (BIMSTEC)
    Colombo Plan
    Mekong–Ganga Cooperation (MGC)
    Mekong River Commission (MRC)
    Partnerships in Environmental Management for the Seas of East Asia (PEMSEA)
    South Asian Association for Regional Cooperation (SAARC)
    Southeast Asian Ministers of Education Organization (SEAMEO)
    Trilateral Cooperation Secretariat (TCS)
    Gulf Cooperation Council (GCC)

  Transcontinental

    Eurasia
        Asia-Europe Foundation (ASEF)
        Central Asian Cooperation Organization
        Collective Security Treaty Organization (CSTO)
        Commonwealth of Independent States (CIS)
        Economic Cooperation Organization (ECO)
        Eurasian Economic Union (EEU or EAEU)
        GUAM Organization for Democracy and Economic Development
        Organization of the Black Sea Economic Cooperation (BSEC)
        Shanghai Cooperation Organisation (SCO)
        TRACECA
        Turkic Council (TurkKon)
    Trans-Atlantic
        North Atlantic Treaty Organization (NATO)
        Organization for Security and Co-operation in Europe (OSCE)
        South Atlantic Peace and Cooperation Zone (ZPCAS)
    Mediterranean
        Union for the Mediterranean
    Indian Ocean
        Indian Ocean Rim Association for Regional Cooperation (IOR-ARC)
        Indian Ocean Commission (COI)
    Arctic Ocean
        Arctic Council
    Pacific Ocean
        ANZUS
        Asia-Pacific Economic Cooperation (APEC)
        Melanesian Spearhead Group (MSG)
        Pacific Islands Forum
        Pacific Regional Environment Programme (SPREP)
        Secretariat of the Pacific Community
    African, Caribbean and Pacific Group of States
        Technical Centre for Agricultural and Rural Cooperation ACP-EU (CTA)

  Africa

    African Union
    Conseil de l'Entente
    Economic Community of West African States (ECOWAS)
    East African Community (EAC)
    West African Economic and Monetary Union (UEMOA)
    Southern African Development Community (SADC)
    Intergovernmental Authority on Development (IGAD)
    Arab Maghreb Union
    Regional Center on Small Arms in the Great Lakes Region, Horn of Africa and Bordering States (RECSA)

  Americas

    Organization of American States (OAS)
    Union of South American Nations (USAN)

        Mercosur
        Andean Community of Nations

    Justice Studies Center of the Americas (JSCA)
    Caribbean Community (CARICOM)
    Association of Caribbean States (ACS)
    Organisation of Eastern Caribbean States (OECS)
    Central American Parliament
    Bolivarian Alliance for the Americas (ALBA)
    Rio Group
    System of Cooperation Among the American Air Forces (SICOFAA)
    Central American Bank for Economic Integration(CABEI)
    Central American Integration System
    Organización Latinoamericana de Energia (OLADE)

Educational organizations and universities

  Academy of European Law (ERA)
  Cerlalc
  Commonwealth of Learning (COL)
  EUCLID (university)
  European University Institute
  International Bureau of Education IBE, now a part of UNESCO
  International Institute for the Unification of Private Law
  United Nations University

Digital organizations

  Digital 5 (D5)

Cultural, ethnic, linguistic, and religious organizations

  Commonwealth of Nations
  International Centre for the Study of the Preservation and Restoration of Cultural Property (ICCROM)
  Organisation internationale de la Francophonie
  Community of Portuguese Language Countries (CPLP)
  Organization of Ibero-American States (OEI)
  Arab League
  Organisation of Islamic Cooperation
  International Organization of Turkic Culture (TÜRKSOY)

Ideological and political groupings

  NATO
  Non-Aligned Movement
  Group of 15 (G-15)
  Group of 77 (G-77)
  Group of 24 (G24)
  Alliance of Small Island States (AOSIS)
  Bolivarian Alliance for the Americas (ALBA)
  New Agenda Coalition
  Non-Proliferation and Disarmament Initiative[2]
  Western European and Others Group

Other

  Advisory Centre on World Trade Organization Law
  Intergovernmental Organisation for International Carriage by Rail (OTIF)
  International Institute for Democracy and Electoral Assistance (International IDEA)
  International Union of Railways (UIC)
  Organization for Cooperation of Railways (OSJD or OSShD)
  Partners in Population and Development
  World Organisation for Animal Health

Defunct

  Allied Control Council
  Arab Cooperation Council
  Central Treaty Organization
  Comecon
  Council of Ambassadors
  Customs and Economic Union of Central Africa
  Eurasian Economic Community (transformed into Eurasian Economic Union)
  French Community
  French Union
  G33
  International Authority for the Ruhr
  International Trade Organization
  Latin League
  Latin Union
  League of Corinth
  League of Nations
  Organisation of African Unity
  Southeast Asia Treaty Organization
  Union of African States

";
    }
}