using System.IO;
using Terraria.ModLoader.IO;
using Microsoft.Xna.Framework;
using System;
using FargowiltasSouls.Common.Utilities;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using FargowiltasSouls.Content.Projectiles;
using FargowiltasSouls.Content.Buffs.Masomode;
using FargowiltasSouls.Core.Systems;
using FargowiltasSouls.Core.Globals;
using FargowiltasSouls.Core.NPCMatching;
using System.Linq;
using static Terraria.ModLoader.PlayerDrawLayer;
using static Terraria.GameContent.Animations.Actions.NPCs;
using System.Collections.Generic;
using System.Reflection;
using Terraria.GameContent.Golf;

namespace FargowiltasSouls.Content.NPCs.EternityModeNPCs.VanillaEnemies.LunarEvents
{
    public abstract class LunarTowers : PillarBehaviour
    {
        public abstract int ShieldStrength { get; set; }

        protected readonly int DebuffNotToInflict;
        protected readonly int AuraDust;
        public int AuraSize = 5000;

        protected LunarTowers(int debuffNotToInflict, int auraDust)
        {
            DebuffNotToInflict = debuffNotToInflict;
            AuraDust = auraDust;
        }
        public abstract void ShieldsDownAI(NPC npc);

        public int AttackTimer;
        public int HealCounter;
        public int AuraSync;
        public bool SpawnedDuringLunarEvent;

        public int Attack = 0;
        public int OldAttack = 0;
        public abstract List<int> RandomAttacks { get; }

        public bool spawned;

        public override void SendExtraAI(NPC npc, BitWriter bitWriter, BinaryWriter binaryWriter)
        {
            base.SendExtraAI(npc, bitWriter, binaryWriter);
            if (!WorldSavingSystem.EternityMode)
            {
                return;
            }
            binaryWriter.Write7BitEncodedInt(AttackTimer);
            binaryWriter.Write7BitEncodedInt(Attack);
            bitWriter.WriteBit(SpawnedDuringLunarEvent);
        }

        public override void ReceiveExtraAI(NPC npc, BitReader bitReader, BinaryReader binaryReader)
        {
            base.ReceiveExtraAI(npc, bitReader, binaryReader);
            if (!WorldSavingSystem.EternityMode)
            {
                return;
            }
            AttackTimer = binaryReader.Read7BitEncodedInt();
            Attack = binaryReader.Read7BitEncodedInt();
            SpawnedDuringLunarEvent = bitReader.ReadBit();
        }

        public override void OnFirstTick(NPC npc)
        {
            base.OnFirstTick(npc);
            if (!WorldSavingSystem.EternityMode)
            {
                return;
            }
            if (npc.type == NPCID.LunarTowerStardust)
            {
                npc.ai[1] = 1000; //disable first tick vanilla constellation spawn
            }
            npc.boss = true;
            npc.buffImmune[BuffID.Suffocation] = true;
            npc.buffImmune[ModContent.BuffType<ClippedWingsBuff>()] = true;
        }
        public bool AnyPlayerWithin(NPC npc, int range)
        {
            foreach (Player p in Main.player.Where(x => x.active && !x.dead))
            {
                if (npc.Distance(p.Center) <= range)
                {
                    return true;
                }
            }
            return false;
        }
        public override void AI(NPC npc)
        {
            bool DontRunAI = npc.type == NPCID.LunarTowerSolar && (Attack == 1);//don't run vanilla AI during solar slam attack or fireball spit attack
            if (!DontRunAI) 
            {
                base.AI(npc);
            }
            if (!WorldSavingSystem.EternityMode)
            {
                return;
            }
            if (npc.type == NPCID.LunarTowerStardust)
            {
                npc.ai[1] = 1000; //disable vanilla constellation spawn
            }
            if (!spawned)
            {
                spawned = true;
                SpawnedDuringLunarEvent = NPC.LunarApocalypseIsUp;
                npc.damage += 150;
                npc.defDamage += 150;
                npc.netUpdate = true;
                npc.buffImmune[ModContent.BuffType<ClippedWingsBuff>()] = true;
            }

            if (SpawnedDuringLunarEvent && ShieldStrength > NPC.LunarShieldPowerMax)
                ShieldStrength = NPC.LunarShieldPowerMax;

            void Aura(int debuff)
            {
                if (DebuffNotToInflict != debuff)
                    EModeGlobalNPC.Aura(npc, AuraSize, debuff, dustid: AuraDust);
            }

            if (SpawnedDuringLunarEvent)
            {
                Aura(ModContent.BuffType<AtrophiedBuff>());
                Aura(ModContent.BuffType<JammedBuff>());
                Aura(ModContent.BuffType<ReverseManaFlowBuff>());
                Aura(ModContent.BuffType<AntisocialBuff>());

                if (++AuraSync > 60)
                {
                    AuraSync -= 600;
                    NetSync(npc);
                }
            }

            if (++HealCounter > 60)
            {
                HealCounter = 0;
                npc.TargetClosest(false);
                if (!npc.HasValidTarget || npc.Distance(Main.player[npc.target].Center) > AuraSize * 1.5f)
                {
                    const int heal = 2000;
                    npc.life += heal;
                    if (npc.life > npc.lifeMax)
                        npc.life = npc.lifeMax;
                    CombatText.NewText(npc.Hitbox, CombatText.HealLife, heal);
                }
            }
            npc.boss = !npc.dontTakeDamage && npc.HasPlayerTarget;
            bool anyPlayersClose = AnyPlayerWithin(npc, AuraSize);
            if (anyPlayersClose)
            {
                AttackTimer++;
            }
            if (npc.dontTakeDamage)
            {
                AuraSize = 5000;
                if (anyPlayersClose)
                {
                    if (ShieldStrength <= 20) //at 20 shield, kill all shield and pillar enemies and go to attack phase
                    {
                        foreach (NPC n in Main.npc)
                        {
                            int[] IDs;
                            switch (npc.type)
                            {
                                case NPCID.LunarTowerSolar:
                                    IDs = Solar.SolarEnemies.SolarEnemyIDs;
                                    break;
                                case NPCID.LunarTowerVortex:
                                    IDs = Vortex.VortexEnemies.VortexEnemyIDs;
                                    break;
                                case NPCID.LunarTowerNebula:
                                    IDs = Nebula.NebulaEnemies.NebulaEnemyIDs;
                                    break;
                                case NPCID.LunarTowerStardust:
                                    IDs = Stardust.StardustEnemies.StardustEnemyIDs;
                                    break;
                                default:
                                    IDs = null;
                                    Main.NewText("You shouldn't be seeing this. Tell Javyz or Terry.");
                                    break;
                            }
                            if (IDs.Contains(n.type))
                            {
                                n.StrikeInstantKill();
                            }
                        }
                        ShieldStrength = 0;
                    }
                }
                if (npc.life < npc.lifeMax)
                {
                    npc.life = npc.lifeMax;
                }
            }
            else
            {
                if (AuraSize > 1500)
                {
                    AuraSize-= 40;
                }
                else
                {
                    AuraSize = 1500;
                }
                if (anyPlayersClose)
                {
                    /*
                    //when shields down, if life is lower than threshhold, put shields back up, go to next non-attacking phase, and ignore rest of AI.
                    if ((float)npc.life / npc.lifeMax < PhaseHealthRatio && Phase >= 0)
                    {
                        Phase = -(Phase + 1);
                        AttackTimer = 0;
                        SoundEngine.PlaySound(SoundID.NPCDeath58, npc.Center);
                        ShieldStrength = NPC.LunarShieldPowerMax;
                        return;
                    }
                    */
                    ShieldsDownAI(npc);
                }
            }
        }

        public override void OnHitPlayer(NPC npc, Player target, Player.HurtInfo hurtInfo)
        {
            base.OnHitPlayer(npc, target, hurtInfo);

            if (!WorldSavingSystem.EternityMode)
            {
                return;
            }

            target.AddBuff(ModContent.BuffType<CurseoftheMoonBuff>(), 600);
        }

        public override void ModifyHitByAnything(NPC npc, Player player, ref NPC.HitModifiers modifiers)
        {
            base.ModifyHitByAnything(npc, player, ref modifiers);

            if (!WorldSavingSystem.EternityMode)
            {
                return;
            }

            if (npc.Distance(player.Center) > AuraSize)
            {
                modifiers.Null();
            }
            else
            {
                modifiers.FinalDamage /= 2;
            }
        }
        #region Help Methods
        public void RandomAttack(NPC npc)
        {
            npc.TargetClosest(false);
            Attack = Main.rand.Next(RandomAttacks);
            while (Attack == OldAttack)
            {
                Attack = Main.rand.Next(RandomAttacks);
            }
            OldAttack = Attack;
            if (npc.life < npc.lifeMax * 0.3f && npc.type == NPCID.LunarTowerVortex)
            {
                Attack = (int)Vortex.LunarTowerVortex.Attacks.VortexVortex;
            }
            AttackTimer = 0;
            NetSync(npc);
        }
        public void EndAttack(NPC npc)
        {
            npc.TargetClosest(false);
            NetSync(npc);
            Attack = 0;
            AttackTimer = 0;
        }
        #endregion
    }
}
